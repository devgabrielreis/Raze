using Raze.Script.Core.Exceptions.InterpreterExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Statements;
using Raze.Script.Core.Statements.Expressions;
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
                return EvaluateIntegerLiteralExpression(expr);
            case NullLiteralExpression expr:
                return EvaluateNullLiteralExpression(expr);

            case IdentifierExpression expr:
                return EvaluateIdentifierExpression(expr, scope);

            case VariableDeclarationStatement stmt:
                return EvaluateVariableDeclarationStatement(stmt, scope);

            case BinaryExpression expr:
                return EvaluateBinaryExpression(expr, scope);
            default:
                throw new UnsupportedStatementException(statement.GetType().Name, statement.StartLine, statement.StartColumn);
        }
    }

    private static RuntimeValue EvaluateProgramExpression(ProgramExpression program, Scope scope)
    {
        RuntimeValue lastValue = new NullValue();

        foreach (var statement in program.Body)
        {
            lastValue = Evaluate(statement, scope);
        }

        return lastValue;
    }

    private static IntegerValue EvaluateIntegerLiteralExpression(IntegerLiteralExpression expression)
    {
        return new IntegerValue(expression.Value);
    }

    private static NullValue EvaluateNullLiteralExpression(NullLiteralExpression expression)
    {
        return new NullValue();
    }

    private static UndefinedValue EvaluateVariableDeclarationStatement(VariableDeclarationStatement statement, Scope scope)
    {
        VariableSymbol variable = statement.Type switch
        {
            IntegerPrimitive => new IntegerVariable(statement.Value is null ? new NullValue() : Evaluate(statement.Value, scope), statement.IsContant),
            _ => throw new Exception("Tipo ainda não suportado")
        };

        scope.DeclareVariable(statement.Identifier, variable, statement);
        return new UndefinedValue();
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
            return variable.Value;
        }

        throw new Exception("nao implementado ainda");
    }
}
