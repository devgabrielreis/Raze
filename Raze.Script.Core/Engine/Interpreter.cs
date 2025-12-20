using Raze.Script.Core.Exceptions.ControlExceptions;
using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Operations;
using Raze.Script.Core.Runtime.Scopes;
using Raze.Script.Core.Runtime.Symbols;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Runtime.Values;
using Raze.Script.Core.Statements;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Statements.Expressions.LiteralExpressions;
using Raze.Script.Core.Tokens.Operators;

namespace Raze.Script.Core.Engine;

internal class Interpreter: IStatementVisitor<Scope, RuntimeValue>
{
    private Runtime.ExecutionContext _executionContext;
    private OperationDispatcher _operationDispatcher;

    public Interpreter()
    {
        _executionContext = new Runtime.ExecutionContext();

        _operationDispatcher = new OperationDispatcher();
        _operationDispatcher.RegisterFrom<IntegerOperationRegistrar>();
        _operationDispatcher.RegisterFrom<DecimalOperationRegistrar>();
        _operationDispatcher.RegisterFrom<StringOperationRegistrar>();
        _operationDispatcher.RegisterFrom<BooleanOperationRegistrar>();
    }

    public RuntimeValue Evaluate(Statement statement, Scope scope)
    {
        return statement.AcceptVisitor(this, scope);
    }

    public RuntimeValue VisitProgramExpression(ProgramExpression program, Scope scope)
    {
        RuntimeValue lastValue = new VoidValue();

        foreach (var statement in program.Body)
        {
            lastValue = Evaluate(statement, scope);
        }

        return lastValue;
    }

    public RuntimeValue VisitVariableDeclarationStatement(VariableDeclarationStatement statement, Scope scope)
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

    public RuntimeValue VisitFunctionDeclarationStatement(FunctionDeclarationStatement statement, Scope scope)
    {
        FunctionValue function = new FunctionValue(
            statement.ReturnType, statement.Parameters, statement.Body, scope
        );

        List<RuntimeType> parameterTypes = statement.Parameters.Select(p => p.Type).ToList();

        FunctionType type = new FunctionType(
            false,
            statement.ReturnType,
            parameterTypes
        );

        VariableSymbol variable = new VariableSymbol(function, type, true, statement.SourceInfo);

        scope.DeclareVariable(statement.Identifier, variable, statement.SourceInfo);

        return new VoidValue();
    }

    public RuntimeValue VisitCallExpression(CallExpression callExpression, Scope scope)
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

            if (!function.Parameters[i].Type.AcceptValue(argumentValue))
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

        _executionContext.EnterFunction();
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
        catch
        {
            _executionContext.ExitFunction(callExpression.SourceInfo);
            throw;
        }

        if (!function.ReturnType.AcceptValue(returnedValue))
        {
            throw new UnexpectedReturnType(
                $"Function was expected to return {function.ReturnType.TypeName} but {returnedValue.TypeName} was returned",
                returnedValueSource
            );
        }

        _executionContext.ExitFunction(returnedValueSource);

        return returnedValue;
    }

    public RuntimeValue VisitAssignmentStatement(AssignmentStatement statement, Scope scope)
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

    public RuntimeValue VisitBinaryExpression(BinaryExpression expression, Scope scope)
    {
        RuntimeValue leftHand = Evaluate(expression.Left, scope);
        OperatorToken op = expression.Operator;
        RuntimeValue rightHand = Evaluate(expression.Right, scope);

        return _operationDispatcher.ExecuteBinaryOperation(leftHand, op, rightHand, expression.SourceInfo);
    }

    public RuntimeValue VisitUnarySimpleExpression(UnarySimpleExpression expression, Scope scope)
    {
        RuntimeValue operand = Evaluate(expression.Operand, scope);
        OperatorToken op = expression.Operator;
        bool isPostfix = expression.IsPostfix;

        return _operationDispatcher.ExecuteUnaryOperation(operand, op, isPostfix, expression.SourceInfo);
    }

    public RuntimeValue VisitUnaryMutationExpression(UnaryMutationExpression expression, Scope scope)
    {
        var variable = GetVariable(expression.Operand, scope);

        var valueBefore = variable.Value;
        var valueAfter = _operationDispatcher.ExecuteUnaryOperation(
            valueBefore, expression.Operator, expression.IsPostfix, expression.SourceInfo
        );

        variable.SetValue(valueAfter, expression.SourceInfo);

        return expression.IsPostfix ? valueBefore : valueAfter;
    }

    public RuntimeValue VisitNullCheckerExpression(NullCheckerExpression expression, Scope scope)
    {
        var runtimeValue = Evaluate(expression.Operand, scope);

        return new BooleanValue(runtimeValue is NullValue);
    }

    public RuntimeValue VisitIdentifierExpression(IdentifierExpression expression, Scope scope)
    {
        var variable = GetVariable(expression, scope);

        return variable.Value;
    }

    public RuntimeValue VisitCodeBlockStatement(CodeBlockStatement codeBlock, Scope scope)
    {
        var codeBlockScope = new LocalScope(scope);

        foreach (var stmt in codeBlock.Body)
        {
            Evaluate(stmt, codeBlockScope);
        }

        return new VoidValue();
    }

    public RuntimeValue VisitIfElseStatement(IfElseStatement ifElse, Scope scope)
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

    public RuntimeValue VisitLoopStatement(LoopStatement loopStmt, Scope scope)
    {
        var outerLoopScope = new LocalScope(scope);

        foreach (var stmt in loopStmt.Initialization)
        {
            Evaluate(stmt, outerLoopScope);
        }

        _executionContext.EnterLoop();

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
                Evaluate(loopStmt.Body, currentIterationScope);
            }
            catch (BreakException)
            {
                break;
            }
            catch (ContinueException)
            {
            }
            catch
            {
                _executionContext.ExitLoop(loopStmt.SourceInfo);
                throw;
            }

            if (loopStmt.Update is not null)
            {
                Evaluate(loopStmt.Update, outerLoopScope);
            }
        }

        _executionContext.ExitLoop(loopStmt.SourceInfo);

        return new VoidValue();
    }

    public RuntimeValue VisitBreakStatement(BreakStatement breakStmt, Scope scope)
    {
        if (!_executionContext.IsInLoop())
        {
            throw new UnexpectedStatementException(
                "Cannot use break outside of a loop",
                breakStmt.SourceInfo
            );
        }

        throw new BreakException(breakStmt.SourceInfo);
    }

    public RuntimeValue VisitContinueStatement(ContinueStatement continueStmt, Scope scope)
    {
        if (!_executionContext.IsInLoop())
        {
            throw new UnexpectedStatementException(
                "Cannot use continue outside of a loop",
                continueStmt.SourceInfo
            );
        }

        throw new ContinueException(continueStmt.SourceInfo);
    }

    public RuntimeValue VisitReturnStatement(ReturnStatement statement, Scope scope)
    {
        if (!_executionContext.IsInFunction())
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

    public RuntimeValue VisitRuntimeValueExpression(RuntimeValueExpression expression, Scope state)
    {
        return expression.Value;
    }

    public RuntimeValue VisitBooleanLiteralExpression(BooleanLiteralExpression expression, Scope state)
    {
        return new BooleanValue(expression.BoolValue);
    }

    public RuntimeValue VisitDecimalLiteralExpression(DecimalLiteralExpression expression, Scope state)
    {
        return new DecimalValue(expression.DecValue);
    }

    public RuntimeValue VisitIntegerLiteralExpression(IntegerLiteralExpression expression, Scope state)
    {
        return new IntegerValue(expression.IntValue);
    }

    public RuntimeValue VisitNullLiteralExpression(NullLiteralExpression expression, Scope state)
    {
        return new NullValue();
    }

    public RuntimeValue VisitStringLiteralExpression(StringLiteralExpression expression, Scope state)
    {
        return new StringValue(expression.StrValue);
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

    private static VariableSymbol GetVariable(IdentifierExpression expression, Scope scope)
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

            return variable;
        }

        throw new Exception("nao implementado ainda");
    }
}
