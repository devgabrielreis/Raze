using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens.Operators;

namespace Raze.Script.Core.Values;

public class IntegerValue : RuntimeValue
{
    public override object? Value => _value;

    public int? IntValue => _value;

    public override string TypeName => "Integer";

    private readonly int? _value;

    public IntegerValue(int? value)
    {
        _value = value;
    }

    internal override RuntimeValue ExecuteBinaryOperation(OperatorToken op, RuntimeValue other, BinaryExpression source)
    {
        if (other is IntegerValue intValue)
        {
            return ExecuteBinaryOperationWithInteger(op, intValue, source);
        }
        else if (other is DecimalValue decimalValue)
        {
            return ExecuteBinaryOperationWithDecimal(op, decimalValue, source);
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
        if (_value is null)
        {
            return "null";
        }

        return _value.ToString()!;
    }

    private RuntimeValue ExecuteBinaryOperationWithInteger(OperatorToken op, IntegerValue other, BinaryExpression source)
    {
        if (IntValue is null || other.IntValue is null)
        {
            throw new NullValueException(source.StartLine, source.StartColumn);
        }

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
        }

        throw new UnsupportedBinaryOperationException(
            TypeName,
            other.TypeName,
            op.Lexeme,
            source.StartLine,
            source.StartColumn
        );
    }

    private RuntimeValue ExecuteBinaryOperationWithDecimal(OperatorToken op, DecimalValue other, BinaryExpression source)
    {
        if (IntValue is null || other.DecValue is null)
        {
            throw new NullValueException(source.StartLine, source.StartColumn);
        }

        switch (op)
        {
            case AdditionOperator:
                return new DecimalValue((decimal)IntValue + other.DecValue);
            case SubtractionOperator:
                return new DecimalValue((decimal)IntValue - other.DecValue);
            case MultiplicationOperator:
                return new DecimalValue((decimal)IntValue * other.DecValue);
            case DivisionOperator:
                if (other.DecValue == 0)
                {
                    throw new DivisionByZeroException(source.StartLine, source.StartColumn);
                }
                return new DecimalValue((decimal)IntValue / other.DecValue);
            case ModuloOperator:
                return new DecimalValue((decimal)IntValue % other.DecValue);
            case EqualOperator:
                return new BooleanValue((decimal)IntValue == other.DecValue);
            case NotEqualOperator:
                return new BooleanValue((decimal)IntValue != other.DecValue);
        }

        throw new UnsupportedBinaryOperationException(
            TypeName,
            other.TypeName,
            op.Lexeme,
            source.StartLine,
            source.StartColumn
        );
    }
}
