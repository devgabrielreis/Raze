using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Context;
using Raze.Script.Core.Runtime.Operations;
using Raze.Script.Core.Runtime.Scopes;
using Raze.Script.Core.Runtime.Symbols;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Runtime.Values;
using Raze.Script.Core.Statements;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Statements.Expressions.LiteralExpressions;

namespace Raze.Script.Core.Engine;

internal sealed class Interpreter: IStatementVisitor<Scope, RuntimeValue>
{
    private Runtime.Context.ExecutionContext _executionContext;
    private OperationDispatcher _operationDispatcher;

    public Interpreter()
    {
        _executionContext = new Runtime.Context.ExecutionContext();

        _operationDispatcher = new OperationDispatcher();
        _operationDispatcher.RegisterFrom<IntegerOperationRegistrar>();
        _operationDispatcher.RegisterFrom<DecimalOperationRegistrar>();
        _operationDispatcher.RegisterFrom<StringOperationRegistrar>();
        _operationDispatcher.RegisterFrom<BooleanOperationRegistrar>();
    }

    public void Evaluate(Statement statement, Scope scope, out RuntimeValue result)
    {
        statement.AcceptVisitor(this, scope, out result);
    }

    public void VisitProgramExpression(ProgramExpression program, Scope scope, out RuntimeValue result)
    {
        result = RuntimeValue.Void;

        foreach (var statement in program.Body)
        {
            Evaluate(statement, scope, out result);
        }
    }

    public void VisitNamespaceDeclarationStatement(
        NamespaceDeclarationStatement statement,
        Scope scope,
        out RuntimeValue result
    )
    {
        Scope namespaceScope;

        if (scope.TryGetNamespace(statement.Identifier) is NamespaceSymbol namespaceSymbol)
        {
            namespaceScope = namespaceSymbol.Scope;
        }
        else
        {
            namespaceScope = Scope.CreateNamespaceScope(scope);
            var newNamespaceSymbol = new NamespaceSymbol(namespaceScope);

            scope.DeclareNamespace(statement.Identifier, newNamespaceSymbol, in statement.SourceInfo);
        }

        foreach (var stmt in statement.DeclarationBlock.Body)
        {
            Evaluate(stmt, namespaceScope, out _);
        }

        result = RuntimeValue.Void;
    }

    public void VisitNamespaceAccessExpression(
        NamespaceAccessExpression expression,
        Scope scope,
        out RuntimeValue result
    )
    {
        var namespaceSymbol = scope.GetNamespace(
            expression.NamespaceIdentifier.Symbol,
            in expression.SourceInfo
        );

        var variable = namespaceSymbol.Scope.GetVariable(
            expression.MemberIdentifier.Symbol,
            in expression.MemberIdentifier.SourceInfo,
            throwIfNotInitialized: true
        );

        result = variable.GetValue(in expression.SourceInfo);
    }

    public void VisitVariableDeclarationStatement(
        VariableDeclarationStatement statement,
        Scope scope,
        out RuntimeValue result
    )
    {
        RuntimeValue? value = null;
        if (statement.Value != null)
        {
            Evaluate(statement.Value, scope, out var value2);
            value = value2;
        }

        VariableSymbol variable = new VariableSymbol(
            value,
            statement.Type,
            statement.IsConstant,
            in statement.SourceInfo
        );

        scope.DeclareVariable(statement.Identifier, variable, in statement.SourceInfo);
        result = RuntimeValue.Void;
    }

    public void VisitFunctionDeclarationStatement(
        FunctionDeclarationStatement statement,
        Scope scope,
        out RuntimeValue result
    )
    {
        UserFunctionValue function = new UserFunctionValue(
            statement.ReturnType, statement.Parameters, statement.Body, scope
        );
        var value = new RuntimeValue(function);

        VariableSymbol variable = new VariableSymbol(value, value.Type, true, in statement.SourceInfo);

        scope.DeclareVariable(statement.Identifier, variable, in statement.SourceInfo);

        result = RuntimeValue.Void;
    }

    public void VisitCallExpression(
        CallExpression callExpression,
        Scope scope,
        out RuntimeValue result
    )
    {
        Evaluate(callExpression.Caller, scope, out var value);

        if (value.Type.Base != BaseType.UserFunction)
        {
            throw new InvalidCallExpressionException(
                $"Cannot call {value.Type}", callExpression.SourceInfo
            );
        }

        var function = value.AsUserFunction();

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

        var functionScope = Scope.CreateLocalScope(function.Scope);

        for (int i = 0; i < function.Parameters.Count; i++)
        {
            RuntimeValue argumentValue;
            SourceInfo argumentSource;

            if (i < callExpression.ArgumentList.Count)
            {
                Evaluate(callExpression.ArgumentList[i], scope, out argumentValue);
                argumentSource = callExpression.ArgumentList[i].SourceInfo;
            }
            else
            {
                Evaluate(function.Parameters[i].DefaultValue!, function.Scope, out argumentValue);
                argumentSource = function.Parameters[i].SourceInfo;
            }

            if (!function.Parameters[i].Type.IsCompatibleWith(in argumentValue))
            {
                throw new InvalidParameterListException(
                    $"Parameter {function.Parameters[i].Identifier} expects type {function.Parameters[i].Type}, but {argumentValue.Type} was given",
                    argumentSource
                );
            }

            var variable = new VariableSymbol(argumentValue, function.Parameters[i].Type, function.Parameters[i].IsConstant, ref argumentSource);

            functionScope.DeclareVariable(function.Parameters[i].Identifier, variable, in argumentSource);
        }

        RuntimeValue returnedValue = RuntimeValue.Void;
        SourceInfo returnedValueSource;

        _executionContext.EnterFunction();

        Evaluate(function.Body, functionScope, out _);
        returnedValueSource = function.Body.SourceInfo;

        if (_executionContext.HasPending(ContextSignal.Return))
        {
            _executionContext.ConsumeReturn(out var returnedSignal);

            if (returnedSignal != null)
            {
                returnedValue = returnedSignal.Value.Value;
                returnedValueSource = returnedSignal.Value.Source;
            }
        }

        _executionContext.ExitFunction(in returnedValueSource);

        if (!function.ReturnType.IsCompatibleWith(in returnedValue))
        {
            throw new UnexpectedReturnType(
                $"Function was expected to return {function.ReturnType} but {returnedValue.Type} was returned",
                returnedValueSource
            );
        }

        result = returnedValue;
    }

    public void VisitAssignmentStatement(
        AssignmentStatement statement,
        Scope scope,
        out RuntimeValue result
    )
    {
        var variable = scope.GetVariable(statement.Target.Symbol, in statement.SourceInfo);
        Evaluate(statement.Value, scope, out var newValue);

        variable.SetValue(ref newValue, in statement.SourceInfo);

        result = RuntimeValue.Void;
    }

    public void VisitBinaryExpression(
        BinaryExpression expression,
        Scope scope,
        out RuntimeValue result
    )
    {
        Evaluate(expression.Left, scope, out var leftHand);
        string op = expression.Operator;
        Evaluate(expression.Right, scope, out var rightHand);
        var source = expression.SourceInfo;

        _operationDispatcher.ExecuteBinaryOperation(ref leftHand, op, ref rightHand, out result, ref source);
    }

    public void VisitUnarySimpleExpression(
        UnarySimpleExpression expression,
        Scope scope,
        out RuntimeValue result
    )
    {
        Evaluate(expression.Operand, scope, out var operand);
        string op = expression.Operator;
        bool isPostfix = expression.IsPostfix;
        var source = expression.SourceInfo;

        _operationDispatcher.ExecuteUnaryOperation(in operand, op, out result, isPostfix, in source);
    }

    public void VisitUnaryMutationExpression(
        UnaryMutationExpression expression,
        Scope scope,
        out RuntimeValue result
    )
    {
        var variable = scope.GetVariable(
            expression.Operand.Symbol, in expression.SourceInfo, throwIfNotInitialized: true
        );

        var valueBefore = variable.GetValue(in expression.SourceInfo);
        _operationDispatcher.ExecuteUnaryOperation(
            in valueBefore, expression.Operator, out var valueAfter, expression.IsPostfix, in expression.SourceInfo
        );

        variable.SetValue(in valueAfter, in expression.SourceInfo);

        result = expression.IsPostfix ? valueBefore : valueAfter;
    }

    public void VisitNullCheckerExpression(
        NullCheckerExpression expression,
        Scope scope,
        out RuntimeValue result
    )
    {
        Evaluate(expression.Operand, scope, out var value);

        result = value.Type == RuntimeType.Null ? RuntimeValue.True : RuntimeValue.False;
    }

    public void VisitIdentifierExpression(
        IdentifierExpression expression,
        Scope scope,
        out RuntimeValue result
    )
    {
        var variable = scope.GetVariable(
            expression.Symbol, in expression.SourceInfo, throwIfNotInitialized: true
        );

        result = variable.GetValue(in expression.SourceInfo);
    }

    public void VisitCodeBlockStatement(
        CodeBlockStatement codeBlock,
        Scope scope,
        out RuntimeValue result
    )
    {
        var codeBlockScope = Scope.CreateLocalScope(scope);

        foreach (var stmt in codeBlock.Body)
        {
            Evaluate(stmt, codeBlockScope, out _);

            if (_executionContext.HasAnyPendingSignal())
            {
                break;
            }
        }

        result = RuntimeValue.Void;
    }

    public void VisitIfElseStatement(
        IfElseStatement ifElse,
        Scope scope,
        out RuntimeValue result
    )
    {
        if (GetValidBooleanValue(ifElse.Condition, scope))
        {
            Evaluate(ifElse.Then, scope, out _);
        }
        else if (ifElse.Else is not null)
        {
            Evaluate(ifElse.Else, scope, out _);
        }

        result = RuntimeValue.Void;
    }

    public void VisitLoopStatement(
        LoopStatement loopStmt,
        Scope scope,
        out RuntimeValue result
    )
    {
        var outerLoopScope = Scope.CreateLocalScope(scope);

        foreach (var stmt in loopStmt.Initialization)
        {
            Evaluate(stmt, outerLoopScope, out _);
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


            var currentIterationScope = Scope.CreateLocalScope(outerLoopScope);
            Evaluate(loopStmt.Body, currentIterationScope, out _);

            if (_executionContext.HasPending(ContextSignal.Break))
            {
                _executionContext.ConsumeBreak();
                break;
            }
            else if (_executionContext.HasPending(ContextSignal.Continue))
            {
                _executionContext.ConsumeContinue();
            }
            else if (_executionContext.HasAnyPendingSignal())
            {
                break;
            }

            if (loopStmt.Update is not null)
            {
                Evaluate(loopStmt.Update, outerLoopScope, out _);
            }
        }

        _executionContext.ExitLoop(in loopStmt.SourceInfo);

        result = RuntimeValue.Void;
    }

    public void VisitBreakStatement(
        BreakStatement breakStmt,
        Scope scope,
        out RuntimeValue result
    )
    {
        if (!_executionContext.IsInLoop())
        {
            throw new UnexpectedStatementException(
                "Cannot use break outside of a loop",
                breakStmt.SourceInfo
            );
        }

        result = RuntimeValue.Void;
        _executionContext.SignalBreak();
    }

    public void VisitContinueStatement(
        ContinueStatement continueStmt,
        Scope scope,
        out RuntimeValue result
    )
    {
        if (!_executionContext.IsInLoop())
        {
            throw new UnexpectedStatementException(
                "Cannot use continue outside of a loop",
                continueStmt.SourceInfo
            );
        }

        result = RuntimeValue.Void;
        _executionContext.SignalContinue();
    }

    public void VisitReturnStatement(
        ReturnStatement statement,
        Scope scope,
        out RuntimeValue result
    )
    {
        if (!_executionContext.IsInFunction())
        {
            throw new UnexpectedStatementException(
                "Cannot use return outside of a function",
                statement.SourceInfo
            );
        }

        ReturnedValue? returnedValue;
        
        if (statement.ReturnedValue != null)
        {
            Evaluate(statement.ReturnedValue, scope, out var returnValue);
            returnedValue = new ReturnedValue(in returnValue, in statement.SourceInfo);
        }
        else
        {
            returnedValue = null;
        }

        result = RuntimeValue.Void;
        _executionContext.SignalReturn(in returnedValue);
    }

    public void VisitRuntimeValueExpression(
        RuntimeValueExpression expression,
        Scope state,
        out RuntimeValue result)
    {
        result = expression.Value;
    }

    public void VisitBooleanLiteralExpression(
        BooleanLiteralExpression expression,
        Scope state,
        out RuntimeValue result)
    {
        result = expression.BoolValue ? RuntimeValue.True : RuntimeValue.False;
    }

    public void VisitDecimalLiteralExpression(
        DecimalLiteralExpression expression,
        Scope state,
        out RuntimeValue result)
    {
        result = new RuntimeValue(expression.DecValue);
    }

    public void VisitIntegerLiteralExpression(
        IntegerLiteralExpression expression,
        Scope state,
        out RuntimeValue result)
    {
        result = new RuntimeValue(expression.IntValue);
    }

    public void VisitNullLiteralExpression(
        NullLiteralExpression expression,
        Scope state,
        out RuntimeValue result)
    {
        result = RuntimeValue.Null;
    }

    public void VisitStringLiteralExpression(
        StringLiteralExpression expression,
        Scope state,
        out RuntimeValue result)
    {
        result = new RuntimeValue(expression.StrValue);
    }

    private bool GetValidBooleanValue(Expression condition, Scope scope)
    {
        Evaluate(condition, scope, out var conditionResult);

        if (conditionResult.Type != RuntimeType.Boolean)
        {
            throw new UnexpectedTypeException(
                conditionResult.GetType().Name,
                RuntimeType.Boolean.ToString(),
                condition.SourceInfo
            );
        }

        return conditionResult.AsBoolean();
    }
}
