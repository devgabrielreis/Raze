using Raze.Script.Core.Constants;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens.Operators;
using Raze.Script.Core.Tokens.Operators.AdditiveOperators;
using Raze.Script.Core.Tokens.Operators.EqualityOperators;
using Raze.Script.Core.Tokens.Operators.MultiplicativeOperators;
using Raze.Script.Core.Tokens.Operators.RelationalOperators;

namespace Raze.Script.Core.Runtime.Values;

public class IntegerValue : RuntimeValue
{
    public override object Value => _value;

    public Int128 IntValue => _value;

    public override string TypeName => TypeNames.INTEGER_TYPE_NAME;

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
            TypeName, other.TypeName, op.Lexeme, source.SourceInfo
        );
    }

    public override string ToString()
    {
        return _value.ToString();
    }

    private RuntimeValue ExecuteBinaryOperationWithInteger(OperatorToken op, IntegerValue other, BinaryExpression source)
    {
        return op switch
        {
            AdditionToken           => new IntegerValue(IntValue + other.IntValue),
            SubtractionToken        => new IntegerValue(IntValue - other.IntValue),
            MultiplicationToken     => new IntegerValue(IntValue * other.IntValue),
            DivisionToken           => other.IntValue == 0
                ? throw new DivisionByZeroException(source.SourceInfo)
                : new IntegerValue(IntValue / other.IntValue),
            ModuloToken             => new IntegerValue(IntValue % other.IntValue),
            EqualToken              => new BooleanValue(IntValue == other.IntValue),
            NotEqualToken           => new BooleanValue(IntValue != other.IntValue),
            GreaterThanToken        => new BooleanValue(IntValue > other.IntValue),
            LessThanToken           => new BooleanValue(IntValue < other.IntValue),
            GreaterOrEqualThanToken => new BooleanValue(IntValue >= other.IntValue),
            LessOrEqualThanToken    => new BooleanValue(IntValue <= other.IntValue),
            _ => throw new UnsupportedBinaryOperationException(
                TypeName, other.TypeName, op.Lexeme, source.SourceInfo
            ),
        };
    }

    public override object Clone()
    {
        return new IntegerValue(_value);
    }
}
