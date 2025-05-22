using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens.Operators;
using System.Globalization;

namespace Raze.Script.Core.Values;

public class DecimalValue : RuntimeValue
{
    public override object? Value => _value;

    public override string TypeName => "Decimal";

    private readonly decimal? _value;

    public DecimalValue(decimal? value)
    {
        _value = value;
    }

    internal override RuntimeValue ExecuteBinaryOperation(OperatorToken op, RuntimeValue other, BinaryExpression source)
    {
        if (other is DecimalValue decimalValue)
        {
            return ExecuteBinaryOperationWithDecimal(op, decimalValue, source);
        }
        else if (other is IntegerValue integerValue)
        {
            return ExecuteBinaryOperationWithInteger(op, integerValue, source);
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
            return "NULL";
        }

        string decimalStr = ((decimal)_value).ToString(CultureInfo.InvariantCulture);

        if (!decimalStr.Contains('.'))
        {
            decimalStr += ".0";
        }

        return decimalStr;
    }

    private DecimalValue ExecuteBinaryOperationWithDecimal(OperatorToken op, DecimalValue other, BinaryExpression source)
    {
        if (Value is null || other.Value is null)
        {
            throw new NullValueException(source.StartLine, source.StartColumn);
        }

        switch (op)
        {
            case AdditionOperator:
                return new DecimalValue((decimal)Value + (decimal)other.Value);
            case SubtractionOperator:
                return new DecimalValue((decimal)Value - (decimal)other.Value);
            case MultiplicationOperator:
                return new DecimalValue((decimal)Value * (decimal)other.Value);
            case DivisionOperator:
                if ((decimal)other.Value == 0)
                {
                    throw new DivisionByZeroException(source.StartLine, source.StartColumn);
                }
                return new DecimalValue((decimal)Value / (decimal)other.Value);
            case ModuloOperator:
                return new DecimalValue((decimal)Value % (decimal)other.Value);
        }

        throw new UnsupportedBinaryOperationException(
            TypeName,
            other.TypeName,
            op.Lexeme,
            source.StartLine,
            source.StartColumn
        );
    }

    private DecimalValue ExecuteBinaryOperationWithInteger(OperatorToken op, IntegerValue other, BinaryExpression source)
    {
        if (Value is null || other.Value is null)
        {
            throw new NullValueException(source.StartLine, source.StartColumn);
        }

        switch (op)
        {
            case AdditionOperator:
                return new DecimalValue((decimal)Value + (decimal)(int)other.Value);
            case SubtractionOperator:
                return new DecimalValue((decimal)Value - (decimal)(int)other.Value);
            case MultiplicationOperator:
                return new DecimalValue((decimal)Value * (decimal)(int)other.Value);
            case DivisionOperator:
                if ((int)other.Value == 0)
                {
                    throw new DivisionByZeroException(source.StartLine, source.StartColumn);
                }
                return new DecimalValue((decimal)Value / (decimal)(int)other.Value);
            case ModuloOperator:
                return new DecimalValue((decimal)Value % (decimal)(int)other.Value);
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
