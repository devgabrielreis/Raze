using Raze.Script.Core.Exceptions.InterpreterExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Statements;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Symbols.Variables;
using Raze.Script.Core.Tokens.Operators;
using Raze.Script.Core.Tokens.Primitives;
using Raze.Script.Core.Types;

namespace Raze.Script.Core.Engine;

internal static class Interpreter
{
    public static RuntimeType Evaluate(Statement statement, Scope scope)
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

    private static RuntimeType EvaluateProgramExpression(ProgramExpression program, Scope scope)
    {
        RuntimeType lastValue = new NullType();

        foreach (var statement in program.Body)
        {
            lastValue = Evaluate(statement, scope);
        }

        return lastValue;
    }

    private static IntegerType EvaluateIntegerLiteralExpression(IntegerLiteralExpression expression)
    {
        return new IntegerType(expression.Value);
    }

    private static NullType EvaluateNullLiteralExpression(NullLiteralExpression expression)
    {
        return new NullType();
    }

    private static NullType EvaluateVariableDeclarationStatement(VariableDeclarationStatement statement, Scope scope)
    {
        VariableSymbol variable = statement.Type switch
        {
            IntegerPrimitive => new IntegerVariable(statement.Value is null ? new NullType() : Evaluate(statement.Value, scope), statement.IsContant),
            _ => throw new Exception("Tipo ainda não suportado")
        };

        scope.DeclareVariable(statement.Identifier, variable, statement);
        return new NullType();
    }

    private static RuntimeType EvaluateBinaryExpression(BinaryExpression expression, Scope scope)
    {
        RuntimeType leftHand = Evaluate(expression.Left, scope);
        OperatorToken op = expression.Operator;
        RuntimeType rightHand = Evaluate(expression.Right, scope);

        return leftHand.ExecuteBinaryOperation(op, rightHand, expression);
    }

    private static RuntimeType EvaluateIdentifierExpression(IdentifierExpression expression, Scope scope)
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
