using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens.Operators;
using Raze.Script.Core.Tokens.Operators.AdditiveOperators;
using Raze.Script.Core.Tokens.Operators.EqualityOperators;
using Raze.Script.Core.Tokens.Operators.MultiplicativeOperators;
using Raze.Script.Core.Tokens.Operators.RelationalOperators;

namespace Raze.Script.Core.Values;

public class IntegerValue : RuntimeValue
{
    public override object Value => _value;

    public Int128 IntValue => _value;

    public override string TypeName => "Integer";

    private readonly Int128 _value;

    public IntegerValue(Int128 value)
    {
        _value = value;
    }

    internal override RuntimeValue ExecuteBinaryOperation(OperatorToken op, RuntimeValue other, BinaryExpression source)
    {
        if (other is IntegerValue intValue)
        {
            return ExecuteBinaryOperationWithInteger(op, intValue, source);
        }

        throw new UnsupportedBinaryOperationException(
            TypeName,
            other.TypeName,
            op.Lexeme,
            source.StartLine,
            source.StartColumn
        );
    }

    public override string ToString()
    {
        return _value.ToString();
    }

    private RuntimeValue ExecuteBinaryOperationWithInteger(OperatorToken op, IntegerValue other, BinaryExpression source)
    {
        switch (op)
        {
            case AdditionOperator:
                return new IntegerValue(IntValue + other.IntValue);
            case SubtractionOperator:
                return new IntegerValue(IntValue - other.IntValue);
            case MultiplicationOperator:
                return new IntegerValue(IntValue * other.IntValue);
            case DivisionOperator:
                if (other.IntValue == 0)
                {
                    throw new DivisionByZeroException(source.StartLine, source.StartColumn);
                }
                return new IntegerValue(IntValue / other.IntValue);
            case ModuloOperator:
                return new IntegerValue(IntValue % other.IntValue);
            case EqualOperator:
                return new BooleanValue(IntValue == other.IntValue);
            case NotEqualOperator:
                return new BooleanValue(IntValue != other.IntValue);
            case GreaterThanOperator:
                return new BooleanValue(IntValue > other.IntValue);
            case LessThanOperator:
                return new BooleanValue(IntValue < other.IntValue);
            case GreaterOrEqualThanOperator:
                return new BooleanValue(IntValue >= other.IntValue);
            case LessOrEqualThanOperator:
                return new BooleanValue(IntValue <= other.IntValue);
        }

        throw new UnsupportedBinaryOperationException(
            TypeName,
            other.TypeName,
            op.Lexeme,
            source.StartLine,
            source.StartColumn
        );
    }

    public override object Clone()
    {
        return new IntegerValue(_value);
    }
}
