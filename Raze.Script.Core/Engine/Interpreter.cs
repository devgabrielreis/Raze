using Raze.Script.Core.Exceptions.InterpreterExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Statements;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Statements.Expressions.LiteralExpressions;
using Raze.Script.Core.Symbols.Variables;
using Raze.Script.Core.Tokens.Operators;
using Raze.Script.Core.Tokens.Primitives;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Engine;

internal static class Interpreter
{
    public static RuntimeValue Evaluate(Statement statement, Scope scope)
    {
        switch (statement)
        {
            case ProgramExpression program:
                return EvaluateProgramExpression(program, scope);

            case IntegerLiteralExpression expr:
                return new IntegerValue(expr.IntValue);
            case DecimalLiteralExpression expr:
                return new DecimalValue(expr.DecValue);
            case BooleanLiteralExpression expr:
                return new BooleanValue(expr.BoolValue);
            case StringLiteralExpression expr:
                return new StringValue(expr.StrValue);
            case NullLiteralExpression:
                return new NullValue();

            case IdentifierExpression expr:
                return EvaluateIdentifierExpression(expr, scope);

            case CodeBlockStatement stmt:
                return EvaluateCodeBlock(stmt, scope);

            case VariableDeclarationStatement stmt:
                return EvaluateVariableDeclarationStatement(stmt, scope);

            case AssignmentStatement stmt:
                return EvaluateAssignmentStatement(stmt, scope);

            case BinaryExpression expr:
                return EvaluateBinaryExpression(expr, scope);

            case IfElseStatement stmt:
                return EvaluateIfElseStatement(stmt, scope);
            case ForStatement stmt:
                return EvaluateForStatement(stmt, scope);

            default:
                throw new UnsupportedStatementException(statement.GetType().Name, statement.StartLine, statement.StartColumn);
        }
    }

    private static RuntimeValue EvaluateProgramExpression(ProgramExpression program, Scope scope)
    {
        RuntimeValue lastValue = new VoidValue();

        foreach (var statement in program.Body)
        {
            lastValue = Evaluate(statement, scope);
        }

        return lastValue;
    }

    private static VoidValue EvaluateVariableDeclarationStatement(VariableDeclarationStatement statement, Scope scope)
    {
        VariableSymbol variable = statement.Type switch
        {
            IntegerPrimitive => new IntegerVariable(
                statement.Value is null ? null : Evaluate(statement.Value, scope),
                statement.IsConstant,
                statement.IsNullable,
                statement.StartLine,
                statement.StartColumn
            ),
            DecimalPrimitive => new DecimalVariable(
                statement.Value is null ? null : Evaluate(statement.Value, scope),
                statement.IsConstant,
                statement.IsNullable,
                statement.StartLine,
                statement.StartColumn
            ),
            BooleanPrimitive => new BooleanVariable(
                statement.Value is null ? null : Evaluate(statement.Value, scope),
                statement.IsConstant,
                statement.IsNullable,
                statement.StartLine,
                statement.StartColumn
            ),
            StringPrimitive => new StringVariable(
                statement.Value is null ? null : Evaluate(statement.Value, scope),
                statement.IsConstant,
                statement.IsNullable,
                statement.StartLine,
                statement.StartColumn
            ),
            _ => throw new Exception("Tipo ainda não suportado")
        };

        scope.DeclareVariable(statement.Identifier, variable, statement);
        return new VoidValue();
    }

    private static VoidValue EvaluateAssignmentStatement(AssignmentStatement statement, Scope scope)
    {
        switch (statement.Target)
        {
            case IdentifierExpression expr:
                scope.AssignVariable(expr.Symbol, Evaluate(statement.Value, scope), statement);
                break;
            default:
                throw new InvalidAssignmentException(statement.StartLine, statement.StartColumn);
        }

        return new VoidValue();
    }

    private static RuntimeValue EvaluateBinaryExpression(BinaryExpression expression, Scope scope)
    {
        RuntimeValue leftHand = Evaluate(expression.Left, scope);
        OperatorToken op = expression.Operator;
        RuntimeValue rightHand = Evaluate(expression.Right, scope);

        return leftHand.ExecuteBinaryOperation(op, rightHand, expression);
    }

    private static RuntimeValue EvaluateIdentifierExpression(IdentifierExpression expression, Scope scope)
    {
        var resolvedScope = scope.Resolve(expression.Symbol);

        if (resolvedScope is null)
        {
            throw new UndefinedIdentifierException(expression.Symbol, expression.StartLine, expression.StartColumn);
        }
            
            
        var result = resolvedScope.Lookup(expression.Symbol);

        if (result is VariableSymbol variable)
        {
            if (!variable.IsInitialized || variable.Value is null)
            {
                throw new UninitializedVariableException(expression.StartLine, expression.StartColumn);
            }

            return variable.Value;
        }

        throw new Exception("nao implementado ainda");
    }

    private static VoidValue EvaluateCodeBlock(CodeBlockStatement codeBlock, Scope scope)
    {
        var codeBlockScope = new LocalScope(scope);

        foreach (var stmt in codeBlock.Body)
        {
            Evaluate(stmt, codeBlockScope);
        }

        return new VoidValue();
    }

    private static VoidValue EvaluateIfElseStatement(IfElseStatement ifElse, Scope scope)
    {
        var conditionResult = Evaluate(ifElse.Condition, scope);

        if (conditionResult is not BooleanValue)
        {
            throw new UnexpectedTypeException(
                conditionResult.GetType().Name,
                nameof(BooleanValue),
                ifElse.Condition.StartLine,
                ifElse.Condition.StartColumn
            );
        }

        if ((conditionResult as BooleanValue)!.BoolValue == null)
        {
            throw new NullValueException(
                ifElse.Condition.StartLine,
                ifElse.Condition.StartColumn
            );
        }

        if ((conditionResult as BooleanValue)!.BoolValue!.Value)
        {
            Evaluate(ifElse.Then, scope);
        }
        else if (ifElse.Else is not null)
        {
            Evaluate(ifElse.Else, scope);
        }

        return new VoidValue();
    }

    public static VoidValue EvaluateForStatement(ForStatement forStmt, Scope scope)
    {
        LocalScope localScope = new LocalScope(scope);

        ExecuteLoop(forStmt.Body, forStmt.Condition, forStmt.Update, forStmt.Initialization, localScope, forStmt);

        return new VoidValue();
    }

    public static void ExecuteLoop(CodeBlockStatement body, Expression? condition, Statement? update, IEnumerable<Statement> initialization, Scope scope, Statement source)
    {
        foreach (var stmt in initialization)
        {
            Evaluate(stmt, scope);
        }

        RuntimeValue? conditionResult = null;

        while (true)
        {
            if (condition is not null)
            {
                conditionResult = Evaluate(condition, scope);

                if (conditionResult is not BooleanValue)
                {
                    throw new UnexpectedTypeException(
                        conditionResult.GetType().Name,
                        nameof(BooleanValue),
                        condition.StartLine,
                        condition.StartColumn
                    );
                }

                if ((conditionResult as BooleanValue)!.BoolValue == null)
                {
                    throw new NullValueException(
                        condition.StartLine,
                        condition.StartColumn
                    );
                }

                if ((conditionResult as BooleanValue)!.BoolValue == false)
                {
                    break;
                }
            }

            EvaluateCodeBlock(body, scope);

            if (update is not null)
            {
                Evaluate(update, scope);
            }
        }
    }
}
