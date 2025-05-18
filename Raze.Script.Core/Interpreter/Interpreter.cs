using Raze.Script.Core.AST;
using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Types;

namespace Raze.Script.Core.Interpreter;

internal static class Interpreter
{
    public static RuntimeType Evaluate(Statement statement)
    {
        switch (statement)
        {
            case ProgramStatement program:
                return EvaluateProgramStatement(program);
            case IntegerLiteralExpression integerLiteralExpression:
                return EvaluateIntegerLiteralExpression(integerLiteralExpression);
            case BinaryExpression binaryExpression:
                return EvaluateBinaryExpression(binaryExpression);
            default:
                throw new InvalidExpressionException("Statement ainda nao suportado pelo interpretador", statement.StartLine, statement.StartColumn);
        }
    }

    private static RuntimeType EvaluateProgramStatement(ProgramStatement program)
    {
        RuntimeType lastValue = new NullType();

        foreach (var statement in program.Body)
        {
            lastValue = Evaluate(statement);
        }

        return lastValue;
    }

    private static IntegerType EvaluateIntegerLiteralExpression(IntegerLiteralExpression expression)
    {
        return new IntegerType(expression.Value);
    }

    private static RuntimeType EvaluateBinaryExpression(BinaryExpression expression)
    {
        RuntimeType leftHand = Evaluate(expression.Left);
        string op = expression.Operator;
        RuntimeType rightHand = Evaluate(expression.Right);

        return leftHand.ExecuteBinaryOperation(op, rightHand, expression);
    }
}
