using Raze.Script.Core.Builders;
using Raze.Script.Core.BuiltInModules;
using Raze.Script.Core.Constants;
using Raze.Script.Core.Exceptions;
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
    private Dictionary<string, Action<ModuleBuilder>>? _customModuleBuilders;

    private Interpreter(Dictionary<string, Action<ModuleBuilder>>? customModuleBuilders)
    {
        if (customModuleBuilders != null)
        {
            ValidateCustomModuleBuilders(customModuleBuilders);
        }

        _customModuleBuilders = customModuleBuilders;

        _executionContext = new Runtime.Context.ExecutionContext();

        _operationDispatcher = new OperationDispatcher();
        _operationDispatcher.RegisterFrom<IntegerOperationRegistrar>();
        _operationDispatcher.RegisterFrom<DecimalOperationRegistrar>();
        _operationDispatcher.RegisterFrom<StringOperationRegistrar>();
        _operationDispatcher.RegisterFrom<BooleanOperationRegistrar>();
    }

    internal static RuntimeValue Evaluate(
        ProgramExpression program,
        Scope scope,
        Dictionary<string, Action<ModuleBuilder>>? customModuleBuilders
    )
    {
        var interpreter = new Interpreter(customModuleBuilders);
        interpreter.EvaluateInternal(program, scope, out var result);

        return result;
    }

    private void EvaluateInternal(Statement statement, Scope scope, out RuntimeValue result)
    {
        statement.AcceptVisitor(this, scope, out result);
    }

    public void VisitProgramExpression(ProgramExpression program, Scope scope, out RuntimeValue result)
    {
        result = RuntimeValue.Void;

        foreach (var statement in program.Body)
        {
            EvaluateInternal(statement, scope, out result);
        }
    }

    public void VisitNamespaceDeclarationStatement(
        NamespaceDeclarationStatement statement,
        Scope scope,
        out RuntimeValue result
    )
    {
        if (BuiltInModuleManager.HasModule(statement.Identifier))
        {
            ThrowHelper.Throw<RedeclarationException>(
                "Cannot create a namespace with the same name as a default module",
                in statement.SourceInfo
            );
        }

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
            EvaluateInternal(stmt, namespaceScope, out _);
        }

        result = RuntimeValue.Void;
    }

    public void VisitImportModuleStatement(
        ImportModuleStatement statement,
        Scope scope,
        out RuntimeValue result
    )
    {
        var module = BuiltInModuleManager.TryGetModule(statement.ModuleName);

        if (module == null)
        {
            module = GetCustomModule(statement.ModuleName);
        }

        if (module == null)
        {
            ThrowHelper.Throw<UndefinedIdentifierException>(
                $"The module \"{statement.ModuleName}\" was not found",
                in statement.SourceInfo
            );
        }

        scope.DeclareNamespace(
            statement.ModuleName,
            module,
            in statement.SourceInfo,
            throwIfAlreadyDeclared: false
        );

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
            EvaluateInternal(statement.Value, scope, out var value2);
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
        EvaluateInternal(callExpression.Caller, scope, out var value);

        switch (value.Type.Base)
        {
            case BaseType.UserFunction:
                CallUserFunction(callExpression, value.AsUserFunction(), scope, out result);
                break;
            case BaseType.SystemFunction:
                CallSystemFunction(callExpression, value.AsSystemFunction(), scope, out result);
                break;
            default:
                ThrowHelper.Throw<InvalidCallExpressionException, RuntimeValue>(
                    $"Cannot call {value.Type}",
                    in callExpression.SourceInfo,
                    out result
                );
                break;
        }
    }

    public void VisitAssignmentStatement(
        AssignmentStatement statement,
        Scope scope,
        out RuntimeValue result
    )
    {
        var variable = GetVariableFromExpression<InvalidAssignmentException>(
            statement.Target,
            scope,
            throwIfNotInitialized: false,
            "Assignment target must be a variable"
        );
        EvaluateInternal(statement.Value, scope, out var newValue);

        variable.SetValue(ref newValue, in statement.SourceInfo);

        result = RuntimeValue.Void;
    }

    public void VisitBinaryExpression(
        BinaryExpression expression,
        Scope scope,
        out RuntimeValue result
    )
    {
        EvaluateInternal(expression.Left, scope, out var leftHand);
        string op = expression.Operator;
        EvaluateInternal(expression.Right, scope, out var rightHand);
        var source = expression.SourceInfo;

        _operationDispatcher.ExecuteBinaryOperation(ref leftHand, op, ref rightHand, out result, ref source);
    }

    public void VisitUnarySimpleExpression(
        UnarySimpleExpression expression,
        Scope scope,
        out RuntimeValue result
    )
    {
        EvaluateInternal(expression.Operand, scope, out var operand);
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
        var variable = GetVariableFromExpression<InvalidOperandException>(
            expression.Operand,
            scope,
            throwIfNotInitialized: true,
            $"The {expression.Operator} operator can only be applied to variables"
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
        var variable = GetVariableFromExpression<InvalidOperandException>(
            expression.Operand,
            scope,
            throwIfNotInitialized: true,
            $"The {Operators.NULL_CHECKER} operator can only be applied to variables"
        );

        var value = variable.GetValue(in expression.Operand.SourceInfo);

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
            EvaluateInternal(stmt, codeBlockScope, out _);

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
            EvaluateInternal(ifElse.Then, scope, out _);
        }
        else if (ifElse.Else is not null)
        {
            EvaluateInternal(ifElse.Else, scope, out _);
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

        if (loopStmt.Initialization != null)
        {
            EvaluateInternal(loopStmt.Initialization, outerLoopScope, out _);
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
            EvaluateInternal(loopStmt.Body, currentIterationScope, out _);

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
                EvaluateInternal(loopStmt.Update, outerLoopScope, out _);
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
            ThrowHelper.Throw<UnexpectedStatementException>(
                "Cannot use break outside of a loop",
                in breakStmt.SourceInfo
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
            ThrowHelper.Throw<UnexpectedStatementException>(
                "Cannot use continue outside of a loop",
                in continueStmt.SourceInfo
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
            ThrowHelper.Throw<UnexpectedStatementException>(
                "Cannot use return outside of a function",
                in statement.SourceInfo
            );
        }

        ReturnedValue? returnedValue;
        
        if (statement.ReturnedValue != null)
        {
            EvaluateInternal(statement.ReturnedValue, scope, out var returnValue);
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

    private static void ValidateCustomModuleBuilders(Dictionary<string, Action<ModuleBuilder>> customModuleBuilders)
    {
        foreach (var customModuleName in customModuleBuilders.Keys)
        {
            if (BuiltInModuleManager.HasModule(customModuleName))
            {
                var source = new SourceInfo($"{nameof(Interpreter)} initializer");
                ThrowHelper.Throw<RedeclarationException>(
                    $"The custom module \"{customModuleName}\" has the same name as a built in module",
                    in source
                );
            }
        }
    }

    private NamespaceSymbol? GetCustomModule(string name)
    {
        if (
            _customModuleBuilders != null
            && _customModuleBuilders.TryGetValue(name, out var moduleBuilderFunction)
        )
        {
            var moduleBuilder = new ModuleBuilder(name);
            moduleBuilderFunction(moduleBuilder);

            return moduleBuilder.Build();
        }

        return null;
    }

    private bool GetValidBooleanValue(Expression condition, Scope scope)
    {
        EvaluateInternal(condition, scope, out var conditionResult);

        if (conditionResult.Type != RuntimeType.Boolean)
        {
            ThrowHelper.Throw<UnexpectedTypeException>(
                $"Expected: {RuntimeType.Boolean}. Found: {conditionResult.Type}",
                in condition.SourceInfo
            );
        }

        return conditionResult.AsBoolean();
    }

    private void CallUserFunction(
        CallExpression callExpression,
        UserFunctionValue userFunction,
        Scope scope,
        out RuntimeValue result
    )
    {
        var parameters = userFunction.Parameters;
        ValidateFunctionParameter(callExpression, parameters);

        var functionScope = Scope.CreateLocalScope(userFunction.Scope);

        for (int i = 0; i < userFunction.Parameters.Count; i++)
        {
            RuntimeValue argumentValue;
            SourceInfo argumentSource;

            if (i < callExpression.ArgumentList.Count)
            {
                EvaluateInternal(callExpression.ArgumentList[i], scope, out argumentValue);
                argumentSource = callExpression.ArgumentList[i].SourceInfo;
            }
            else
            {
                EvaluateInternal(userFunction.Parameters[i].DefaultValue!, userFunction.Scope, out argumentValue);
                argumentSource = userFunction.Parameters[i].SourceInfo;
            }

            if (!userFunction.Parameters[i].Type.IsCompatibleWith(in argumentValue))
            {
                ThrowHelper.Throw<InvalidParameterListException>(
                    $"Parameter {userFunction.Parameters[i].Identifier} expects type {userFunction.Parameters[i].Type}, but {argumentValue.Type} was given",
                    in argumentSource
                );
            }

            var variable = new VariableSymbol(argumentValue, userFunction.Parameters[i].Type, userFunction.Parameters[i].IsConstant, ref argumentSource);

            functionScope.DeclareVariable(userFunction.Parameters[i].Identifier, variable, in argumentSource);
        }

        RuntimeValue returnedValue = RuntimeValue.Void;
        SourceInfo returnedValueSource;

        _executionContext.EnterFunction();

        EvaluateInternal(userFunction.Body, functionScope, out _);
        returnedValueSource = userFunction.Body.SourceInfo;

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

        if (!userFunction.ReturnType.IsCompatibleWith(in returnedValue))
        {
            ThrowHelper.Throw<UnexpectedReturnType>(
                $"Function was expected to return {userFunction.ReturnType} but {returnedValue.Type} was returned",
                in returnedValueSource
            );
        }

        result = returnedValue;
    }

    private void CallSystemFunction(
        CallExpression callExpression,
        SystemFunctionValue systemFunction,
        Scope scope,
        out RuntimeValue result
    )
    {
        var parameters = systemFunction.Parameters;
        ValidateFunctionParameter(callExpression, parameters);

        var functionParameters = new RazeFunctionParameters();

        for (int i = 0; i < systemFunction.Parameters.Count; i++)
        {
            RuntimeValue argumentValue;
            SourceInfo argumentSource;

            if (i < callExpression.ArgumentList.Count)
            {
                EvaluateInternal(callExpression.ArgumentList[i], scope, out argumentValue);
                argumentSource = callExpression.ArgumentList[i].SourceInfo;
            }
            else
            {
                EvaluateInternal(systemFunction.Parameters[i].DefaultValue!, scope, out argumentValue);
                argumentSource = callExpression.SourceInfo;
            }

            if (!systemFunction.Parameters[i].Type.IsCompatibleWith(in argumentValue))
            {
                ThrowHelper.Throw<InvalidParameterListException>(
                    $"Parameter {systemFunction.Parameters[i].Identifier} expects type {systemFunction.Parameters[i].Type}, but {argumentValue.Type} was given",
                    in argumentSource
                );
            }

            functionParameters.Add(systemFunction.Parameters[i].Identifier, argumentValue.AsObject());
        }

        var returnValueWrapper = systemFunction.Body(functionParameters);

        if (!systemFunction.ReturnType.IsCompatibleWith(in returnValueWrapper.Value))
        {
            ThrowHelper.Throw<UnexpectedReturnType>(
                $"Function was expected to return {systemFunction.ReturnType} but {returnValueWrapper.Value.Type} was returned",
                in callExpression.SourceInfo
            );
        }

        result = returnValueWrapper.Value;
    }

    private static void ValidateFunctionParameter(
        CallExpression callExpression,
        IReadOnlyList<ParameterSymbol> functionParameters
    )
    {
        int minExpectedArguments = functionParameters.Count(p => !p.HasDefaultValue());

        if (callExpression.ArgumentList.Count < minExpectedArguments || callExpression.ArgumentList.Count > functionParameters.Count)
        {
            string message = minExpectedArguments == functionParameters.Count
                                ? $"Function expects {functionParameters.Count} arguments, but {callExpression.ArgumentList.Count} were given"
                                : $"Function expects between {minExpectedArguments} and {functionParameters.Count} arguments, but {callExpression.ArgumentList.Count} were given";

            ThrowHelper.Throw<InvalidParameterListException>(
                message, in callExpression.SourceInfo
            );
        }
    }

    private static VariableSymbol GetVariableFromExpression<TErrorIfNotVariable>(
        Expression expr,
        Scope scope,
        bool throwIfNotInitialized,
        string notVariableErrorMessage
    )
        where TErrorIfNotVariable : RazeException, IThrowableByThrowHelper<TErrorIfNotVariable>
    {
        switch (expr)
        {
            case IdentifierExpression identifierExpression:
                return scope.GetVariable(
                    identifierExpression.Symbol,
                    in identifierExpression.SourceInfo,
                    throwIfNotInitialized
                );
            case NamespaceAccessExpression namespaceAccessExpression:
                var namespaceSymbol = scope.GetNamespace(
                    namespaceAccessExpression.NamespaceIdentifier.Symbol,
                    in namespaceAccessExpression.SourceInfo
                );

                return namespaceSymbol.Scope.GetVariable(
                    namespaceAccessExpression.MemberIdentifier.Symbol,
                    in namespaceAccessExpression.SourceInfo,
                    throwIfNotInitialized
                );
            default:
                return ThrowHelper.Throw<TErrorIfNotVariable, VariableSymbol>(
                    notVariableErrorMessage,
                    in expr.SourceInfo
                );
        }
    }
}
