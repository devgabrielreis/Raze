using Raze.Script.Core.Exceptions.ControlExceptions;
using Raze.Script.Core.Exceptions.InterpreterExceptions;
using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Statements;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Statements.Expressions.LiteralExpressions;
using Raze.Script.Core.Symbols;
using Raze.Script.Core.Tokens.Operators;
using Raze.Script.Core.Types;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Engine;

internal class Interpreter
{
    private enum ExecutionContexts
    {
        Loop,
        Function
    }

    private Stack<ExecutionContexts> _contextStack;

    public Interpreter()
    {
        _contextStack = new Stack<ExecutionContexts>();
    }

    public void Reset()
    {
        _contextStack.Clear();
    }

    public RuntimeValue Evaluate(Statement statement, Scope scope)
    {
        return statement switch
        {
            ProgramExpression         program => EvaluateProgramExpression(program, scope),
            NullLiteralExpression             => new NullValue(),
            IntegerLiteralExpression     expr => new IntegerValue(expr.IntValue),
            DecimalLiteralExpression     expr => new DecimalValue(expr.DecValue),
            BooleanLiteralExpression     expr => new BooleanValue(expr.BoolValue),
            StringLiteralExpression      expr => new StringValue(expr.StrValue),
            IdentifierExpression         expr => EvaluateIdentifierExpression(expr, scope),
            CodeBlockStatement           stmt => EvaluateCodeBlock(stmt, scope),
            VariableDeclarationStatement stmt => EvaluateVariableDeclarationStatement(stmt, scope),
            FunctionDeclarationStatement stmt => EvaluateFunctionDeclarationStatement(stmt, scope),
            AssignmentStatement          stmt => EvaluateAssignmentStatement(stmt, scope),
            CallExpression               expr => EvaluateCallExpression(expr, scope),
            BinaryExpression             expr => EvaluateBinaryExpression(expr, scope),
            IfElseStatement              stmt => EvaluateIfElseStatement(stmt, scope),
            LoopStatement                stmt => EvaluateLoopStatement(stmt, scope),
            BreakStatement               stmt => EvaluateBreakStatement(stmt),
            ContinueStatement            stmt => EvaluateContinueStatement(stmt),
            ReturnStatement              stmt => EvaluateReturnStatement(stmt, scope),
            RuntimeValueExpression       expr => expr.Value,
            _ => throw new UnsupportedStatementException(
                statement.GetType().Name, statement.SourceInfo
            )
        };
    }

    private RuntimeValue EvaluateProgramExpression(ProgramExpression program, Scope scope)
    {
        RuntimeValue lastValue = new VoidValue();

        foreach (var statement in program.Body)
        {
            lastValue = Evaluate(statement, scope);
        }

        return lastValue;
    }

    private VoidValue EvaluateVariableDeclarationStatement(VariableDeclarationStatement statement, Scope scope)
    {
        VariableSymbol variable = new VariableSymbol(
            statement.Value is null ? null : Evaluate(statement.Value, scope),
            statement.Type,
            statement.IsConstant,
            statement.SourceInfo
        );

        scope.DeclareVariable(statement.Identifier, variable, statement.SourceInfo);
        return new VoidValue();
    }

    private VoidValue EvaluateFunctionDeclarationStatement(FunctionDeclarationStatement statement, Scope scope)
    {
        FunctionValue function = new FunctionValue(
            statement.ReturnType, statement.Parameters, statement.Body, scope
        );

        FunctionType type = new FunctionType(false, statement.ReturnType, statement.Parameters);

        VariableSymbol variable = new VariableSymbol(function, type, true, statement.SourceInfo);

        scope.DeclareVariable(statement.Identifier, variable, statement.SourceInfo);

        return new VoidValue();
    }

    private RuntimeValue EvaluateCallExpression(CallExpression callExpression, Scope scope)
    {
        var value = Evaluate(callExpression.Caller, scope);

        if (value is not FunctionValue function)
        {
            throw new InvalidCallExpressionException(
                $"Cannot call {value.TypeName}", callExpression.SourceInfo
            );
        }

        var parameters = function.Parameters;
        int minExpectedArguments = parameters.Count(p => !p.HasDefaultValue());

        if (callExpression.ArgumentList.Count < minExpectedArguments || callExpression.ArgumentList.Count > parameters.Count)
        {
            string message = minExpectedArguments == parameters.Count
                                ? $"Function expects {parameters.Count} arguments, but {callExpression.ArgumentList.Count} were given"
                                : $"Function expects between {minExpectedArguments} and {parameters.Count} arguments, but {callExpression.ArgumentList.Count} were given";

            throw new InvalidParameterListException(
                message, callExpression.SourceInfo
            );
        }

        var functionScope = new LocalScope(function.Scope);

        for (int i = 0; i < function.Parameters.Count; i++)
        {
            RuntimeValue argumentValue;
            SourceInfo argumentSource;

            if (i < callExpression.ArgumentList.Count)
            {
                argumentValue = Evaluate(callExpression.ArgumentList[i], scope);
                argumentSource = callExpression.ArgumentList[i].SourceInfo;
            }
            else
            {
                argumentValue = Evaluate(function.Parameters[i].DefaultValue!, function.Scope);
                argumentSource = function.Parameters[i].SourceInfo;
            }

            if (!function.Parameters[i].Type.Accept(argumentValue))
            {
                throw new InvalidParameterListException(
                    $"Parameter {function.Parameters[i].Identifier} expects type {function.Parameters[i].Type.TypeName}, but {argumentValue.TypeName} was given",
                    argumentSource
                );
            }

            var variable = new VariableSymbol(argumentValue, function.Parameters[i].Type, function.Parameters[i].IsConstant, argumentSource);

            functionScope.DeclareVariable(function.Parameters[i].Identifier, variable, argumentSource);
        }

        RuntimeValue returnedValue = new VoidValue();
        SourceInfo returnedValueSource;

        _contextStack.Push(ExecutionContexts.Function);
        try
        {
            Evaluate(function.Body, functionScope);
            returnedValueSource = function.Body.SourceInfo;
        }
        catch (ReturnException returnException)
        {
            returnedValue = returnException.ReturnedValue;
            returnedValueSource = returnException.SourceInfo;
        }

        if (!function.ReturnType.Accept(returnedValue))
        {
            throw new UnexpectedReturnType(
                $"Function was expected to return {function.ReturnType.TypeName} but {returnedValue.TypeName} was returned",
                returnedValueSource
            );
        }

        if (!_contextStack.Contains(ExecutionContexts.Function))
        {
            throw new Exception("Corrupted context stack");
        }

        while (true)
        {
            var removedContext = _contextStack.Pop();

            if (removedContext == ExecutionContexts.Function)
            {
                break;
            }
        }

        return returnedValue;
    }

    private VoidValue EvaluateAssignmentStatement(AssignmentStatement statement, Scope scope)
    {
        switch (statement.Target)
        {
            case IdentifierExpression expr:
                scope.AssignVariable(expr.Symbol, Evaluate(statement.Value, scope), statement.SourceInfo);
                break;
            default:
                throw new InvalidAssignmentException(statement.SourceInfo);
        }

        return new VoidValue();
    }

    private RuntimeValue EvaluateBinaryExpression(BinaryExpression expression, Scope scope)
    {
        RuntimeValue leftHand = Evaluate(expression.Left, scope);
        OperatorToken op = expression.Operator;
        RuntimeValue rightHand = Evaluate(expression.Right, scope);

        return leftHand.ExecuteBinaryOperation(op, rightHand, expression);
    }

    private static RuntimeValue EvaluateIdentifierExpression(IdentifierExpression expression, Scope scope)
    {
        var resolvedScope = scope.FindSymbolScope(expression.Symbol);

        if (resolvedScope is null)
        {
            throw new UndefinedIdentifierException(expression.Symbol, expression.SourceInfo);
        }
 
        var result = resolvedScope.FindSymbol(expression.Symbol);

        if (result is VariableSymbol variable)
        {
            if (!variable.IsInitialized)
            {
                throw new UninitializedVariableException(expression.SourceInfo);
            }

            return variable.Value;
        }

        throw new Exception("nao implementado ainda");
    }

    private VoidValue EvaluateCodeBlock(CodeBlockStatement codeBlock, Scope scope)
    {
        var codeBlockScope = new LocalScope(scope);

        foreach (var stmt in codeBlock.Body)
        {
            Evaluate(stmt, codeBlockScope);
        }

        return new VoidValue();
    }

    private VoidValue EvaluateIfElseStatement(IfElseStatement ifElse, Scope scope)
    {
        if (GetValidBooleanValue(ifElse.Condition, scope))
        {
            Evaluate(ifElse.Then, scope);
        }
        else if (ifElse.Else is not null)
        {
            Evaluate(ifElse.Else, scope);
        }

        return new VoidValue();
    }

    public VoidValue EvaluateLoopStatement(LoopStatement loopStmt, Scope scope)
    {
        var outerLoopScope = new LocalScope(scope);

        foreach (var stmt in loopStmt.Initialization)
        {
            Evaluate(stmt, outerLoopScope);
        }

        _contextStack.Push(ExecutionContexts.Loop);

        while (true)
        {
            if (loopStmt.Condition is not null)
            {
                if (!GetValidBooleanValue(loopStmt.Condition, outerLoopScope))
                {
                    break;
                }
            }

            try
            {
                var currentIterationScope = new LocalScope(outerLoopScope);
                EvaluateCodeBlock(loopStmt.Body, currentIterationScope);
            }
            catch (BreakException)
            {
                break;
            }
            catch (ContinueException)
            {
            }

            if (loopStmt.Update is not null)
            {
                Evaluate(loopStmt.Update, outerLoopScope);
            }
        }

        if (_contextStack.Count == 0 || _contextStack.Peek() != ExecutionContexts.Loop)
        {
            throw new Exception("Corrupted context stack");
        }

        _contextStack.Pop();

        return new VoidValue();
    }

    public VoidValue EvaluateBreakStatement(BreakStatement breakStmt)
    {
        if (_contextStack.Count == 0 || _contextStack.Peek() != ExecutionContexts.Loop)
        {
            throw new UnexpectedStatementException(
                "Cannot use break outside of a loop",
                breakStmt.SourceInfo
            );
        }

        throw new BreakException(breakStmt.SourceInfo);
    }

    public VoidValue EvaluateContinueStatement(ContinueStatement continueStmt)
    {
        if (_contextStack.Count == 0 || _contextStack.Peek() != ExecutionContexts.Loop)
        {
            throw new UnexpectedStatementException(
                "Cannot use continue outside of a loop",
                continueStmt.SourceInfo
            );
        }

        throw new ContinueException(continueStmt.SourceInfo);
    }

    public VoidValue EvaluateReturnStatement(ReturnStatement statement, Scope scope)
    {
        if (!_contextStack.Contains(ExecutionContexts.Function))
        {
            throw new UnexpectedStatementException(
                "Cannot use return outside of a function",
                statement.SourceInfo
            );
        }

        RuntimeValue returnedValue = (statement.ReturnedValue is null)
                                        ? new VoidValue()
                                        : Evaluate(statement.ReturnedValue, scope);

        throw new ReturnException(returnedValue, statement.SourceInfo);
    }

    private bool GetValidBooleanValue(Expression condition, Scope scope)
    {
        var conditionResult = Evaluate(condition, scope);

        if (conditionResult is not BooleanValue)
        {
            throw new UnexpectedTypeException(
                conditionResult.GetType().Name,
                nameof(BooleanValue),
                condition.SourceInfo
            );
        }

        return (conditionResult as BooleanValue)!.BoolValue;
    }
}
