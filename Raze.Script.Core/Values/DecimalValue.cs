using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens.Operators;
using Raze.Script.Core.Tokens.Operators.AdditiveOperators;
using Raze.Script.Core.Tokens.Operators.EqualityOperators;
using Raze.Script.Core.Tokens.Operators.MultiplicativeOperators;
using Raze.Script.Core.Tokens.Operators.RelationalOperators;
using System.Globalization;

namespace Raze.Script.Core.Values;

public class DecimalValue : RuntimeValue
{
    public override object Value => _value;

    public decimal DecValue => _value;

    public override string TypeName => "Decimal";

    private readonly decimal _value;

    public DecimalValue(decimal value)
    {
        _value = value;
    }

    internal override RuntimeValue ExecuteBinaryOperation(OperatorToken op, RuntimeValue other, BinaryExpression source)
    {
        if (other is DecimalValue decimalValue)
        {
            return ExecuteBinaryOperationWithDecimal(op, decimalValue, source);
        }

        throw new UnsupportedBinaryOperationException(
            TypeName, other.TypeName, op.Lexeme, source.StartLine, source.StartColumn
        );
    }

    public override string ToString()
    {
        string decimalStr = ((decimal)_value).ToString(CultureInfo.InvariantCulture);

        if (!decimalStr.Contains('.'))
        {
            decimalStr += ".0";
        }

        return decimalStr;
    }

    private RuntimeValue ExecuteBinaryOperationWithDecimal(OperatorToken op, DecimalValue other, BinaryExpression source)
    {
        return op switch
        {
            AdditionToken           => new DecimalValue(DecValue + other.DecValue),
            SubtractionToken        => new DecimalValue(DecValue - other.DecValue),
            MultiplicationToken     => new DecimalValue(DecValue * other.DecValue),
            DivisionToken           => other.DecValue == 0
                ? throw new DivisionByZeroException(source.StartLine, source.StartColumn)
                : new DecimalValue(DecValue / other.DecValue),
            ModuloToken             => new DecimalValue(DecValue % other.DecValue),
            EqualToken              => new BooleanValue(DecValue == other.DecValue),
            NotEqualToken           => new BooleanValue(DecValue != other.DecValue),
            GreaterThanToken        => new BooleanValue(DecValue > other.DecValue),
            LessThanToken           => new BooleanValue(DecValue < other.DecValue),
            GreaterOrEqualThanToken => new BooleanValue(DecValue >= other.DecValue),
            LessOrEqualThanToken    => new BooleanValue(DecValue <= other.DecValue),
            _ => throw new UnsupportedBinaryOperationException(
                TypeName, other.TypeName, op.Lexeme, source.StartLine, source.StartColumn
            ),
        };
    }

    public override object Clone()
    {
        return new DecimalValue(_value);
    }
}
