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

namespace Raze.Script.Core.Engine;

internal sealed class Interpreter: IStatementVisitor<Scope, RuntimeValue>
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
        RuntimeValue lastValue = RuntimeValue.Void;

        foreach (var statement in program.Body)
        {
            lastValue = Evaluate(statement, scope);
        }

        return lastValue;
    }

    public RuntimeValue VisitNamespaceDeclarationStatement(NamespaceDeclarationStatement statement, Scope scope)
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

            scope.DeclareNamespace(statement.Identifier, newNamespaceSymbol, statement.SourceInfo);
        }

        foreach (var stmt in statement.DeclarationBlock.Body)
        {
            Evaluate(stmt, namespaceScope);
        }

        return RuntimeValue.Void;
    }

    public RuntimeValue VisitNamespaceAccessExpression(NamespaceAccessExpression expression, Scope scope)
    {
        var namespaceSymbol = scope.GetNamespace(
            expression.NamespaceIdentifier.Symbol,
            expression.SourceInfo
        );

        var variable = namespaceSymbol.Scope.GetVariable(
            expression.MemberIdentifier.Symbol,
            expression.MemberIdentifier.SourceInfo,
            throwIfNotInitialized: true
        );

        var source = expression.SourceInfo;
        return variable.GetValue(ref source);
    }

    public RuntimeValue VisitVariableDeclarationStatement(VariableDeclarationStatement statement, Scope scope)
    {
        var source = statement.SourceInfo;
        VariableSymbol variable = new VariableSymbol(
            statement.Value is null ? null : Evaluate(statement.Value, scope),
            statement.Type,
            statement.IsConstant,
            ref source
        );

        scope.DeclareVariable(statement.Identifier, variable, statement.SourceInfo);
        return RuntimeValue.Void;
    }

    public RuntimeValue VisitFunctionDeclarationStatement(FunctionDeclarationStatement statement, Scope scope)
    {
        UserFunctionValue function = new UserFunctionValue(
            statement.ReturnType, statement.Parameters, statement.Body, scope
        );
        var value = new RuntimeValue(function);
        var source = statement.SourceInfo;

        VariableSymbol variable = new VariableSymbol(value, value.Type, true, in source);

        scope.DeclareVariable(statement.Identifier, variable, statement.SourceInfo);

        return RuntimeValue.Void;
    }

    public RuntimeValue VisitCallExpression(CallExpression callExpression, Scope scope)
    {
        var value = Evaluate(callExpression.Caller, scope);

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
                argumentValue = Evaluate(callExpression.ArgumentList[i], scope);
                argumentSource = callExpression.ArgumentList[i].SourceInfo;
            }
            else
            {
                argumentValue = Evaluate(function.Parameters[i].DefaultValue!, function.Scope);
                argumentSource = function.Parameters[i].SourceInfo;
            }

            if (!function.Parameters[i].Type.IsCompatibleWith(ref argumentValue))
            {
                throw new InvalidParameterListException(
                    $"Parameter {function.Parameters[i].Identifier} expects type {function.Parameters[i].Type}, but {argumentValue.Type} was given",
                    argumentSource
                );
            }

            var variable = new VariableSymbol(argumentValue, function.Parameters[i].Type, function.Parameters[i].IsConstant, ref argumentSource);

            functionScope.DeclareVariable(function.Parameters[i].Identifier, variable, argumentSource);
        }

        RuntimeValue returnedValue = RuntimeValue.Void;
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

        if (!function.ReturnType.IsCompatibleWith(ref returnedValue))
        {
            throw new UnexpectedReturnType(
                $"Function was expected to return {function.ReturnType} but {returnedValue.Type} was returned",
                returnedValueSource
            );
        }

        _executionContext.ExitFunction(returnedValueSource);

        return returnedValue;
    }

    public RuntimeValue VisitAssignmentStatement(AssignmentStatement statement, Scope scope)
    {
        var variable = scope.GetVariable(statement.Target.Symbol, statement.SourceInfo);
        var newValue = Evaluate(statement.Value, scope);

        var source = statement.SourceInfo;

        variable.SetValue(ref newValue, ref source);

        return RuntimeValue.Void;
    }

    public RuntimeValue VisitBinaryExpression(BinaryExpression expression, Scope scope)
    {
        RuntimeValue leftHand = Evaluate(expression.Left, scope);
        string op = expression.Operator;
        RuntimeValue rightHand = Evaluate(expression.Right, scope);
        var source = expression.SourceInfo;

        _operationDispatcher.ExecuteBinaryOperation(ref leftHand, op, ref rightHand, out var result, ref source);

        return result;
    }

    public RuntimeValue VisitUnarySimpleExpression(UnarySimpleExpression expression, Scope scope)
    {
        RuntimeValue operand = Evaluate(expression.Operand, scope);
        string op = expression.Operator;
        bool isPostfix = expression.IsPostfix;
        var source = expression.SourceInfo;

        _operationDispatcher.ExecuteUnaryOperation(in operand, op, out var result, isPostfix, in source);

        return result;
    }

    public RuntimeValue VisitUnaryMutationExpression(UnaryMutationExpression expression, Scope scope)
    {
        var source = expression.SourceInfo;
        var variable = scope.GetVariable(
            expression.Operand.Symbol, expression.SourceInfo, throwIfNotInitialized: true
        );

        var valueBefore = variable.GetValue(in source);
        _operationDispatcher.ExecuteUnaryOperation(
            in valueBefore, expression.Operator, out var valueAfter, expression.IsPostfix, in source
        );

        variable.SetValue(in valueAfter, in source);

        return expression.IsPostfix ? valueBefore : valueAfter;
    }

    public RuntimeValue VisitNullCheckerExpression(NullCheckerExpression expression, Scope scope)
    {
        var value = Evaluate(expression.Operand, scope);

        return value.Type == RuntimeType.Null ? RuntimeValue.True : RuntimeValue.False;
    }

    public RuntimeValue VisitIdentifierExpression(IdentifierExpression expression, Scope scope)
    {
        var source = expression.SourceInfo;
        var variable = scope.GetVariable(
            expression.Symbol, expression.SourceInfo, throwIfNotInitialized: true
        );

        return variable.GetValue(in source);
    }

    public RuntimeValue VisitCodeBlockStatement(CodeBlockStatement codeBlock, Scope scope)
    {
        var codeBlockScope = Scope.CreateLocalScope(scope);

        foreach (var stmt in codeBlock.Body)
        {
            Evaluate(stmt, codeBlockScope);
        }

        return RuntimeValue.Void;
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

        return RuntimeValue.Void;
    }

    public RuntimeValue VisitLoopStatement(LoopStatement loopStmt, Scope scope)
    {
        var outerLoopScope = Scope.CreateLocalScope(scope);

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
                var currentIterationScope = Scope.CreateLocalScope(outerLoopScope);
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

        return RuntimeValue.Void;
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
                                        ? RuntimeValue.Void
                                        : Evaluate(statement.ReturnedValue, scope);

        throw new ReturnException(returnedValue, statement.SourceInfo);
    }

    public RuntimeValue VisitRuntimeValueExpression(RuntimeValueExpression expression, Scope state)
    {
        return expression.Value;
    }

    public RuntimeValue VisitBooleanLiteralExpression(BooleanLiteralExpression expression, Scope state)
    {
        return expression.BoolValue ? RuntimeValue.True : RuntimeValue.False;
    }

    public RuntimeValue VisitDecimalLiteralExpression(DecimalLiteralExpression expression, Scope state)
    {
        return new RuntimeValue(expression.DecValue);
    }

    public RuntimeValue VisitIntegerLiteralExpression(IntegerLiteralExpression expression, Scope state)
    {
        return new RuntimeValue(expression.IntValue);
    }

    public RuntimeValue VisitNullLiteralExpression(NullLiteralExpression expression, Scope state)
    {
        return RuntimeValue.Null;
    }

    public RuntimeValue VisitStringLiteralExpression(StringLiteralExpression expression, Scope state)
    {
        return new RuntimeValue(expression.StrValue);
    }

    private bool GetValidBooleanValue(Expression condition, Scope scope)
    {
        var conditionResult = Evaluate(condition, scope);

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
