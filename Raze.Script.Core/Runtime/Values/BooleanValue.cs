using Raze.Script.Core.Constants;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens.Operators;
using Raze.Script.Core.Tokens.Operators.EqualityOperators;

namespace Raze.Script.Core.Runtime.Values;

public class BooleanValue : RuntimeValue
{
    public override object Value => _value;

    public bool BoolValue => _value;

    public override string TypeName => TypeNames.BOOLEAN_TYPE_NAME;

    private readonly bool _value;

    public BooleanValue(bool value)
    {
        _value = value;
    }

    internal override RuntimeValue ExecuteBinaryOperation(OperatorToken op, RuntimeValue other, BinaryExpression source)
    {
        if (other is BooleanValue boolValue)
        {
            return ExecuteBinaryOperationWithBoolean(op, boolValue, source);
        }

        throw new UnsupportedBinaryOperationException(
            TypeName, other.TypeName, op.Lexeme, source.SourceInfo
        );
    }

    private BooleanValue ExecuteBinaryOperationWithBoolean(OperatorToken op, BooleanValue other, BinaryExpression source)
    {
        return op switch
        {
            EqualToken    => new BooleanValue(BoolValue == other.BoolValue),
            NotEqualToken => new BooleanValue(BoolValue != other.BoolValue),
            AndToken      => new BooleanValue(BoolValue && other.BoolValue),
            OrToken       => new BooleanValue(BoolValue || other.BoolValue),
            _ => throw new UnsupportedBinaryOperationException(
                TypeName, other.TypeName, op.Lexeme, source.SourceInfo
            ),
        };
    }

    public override string ToString()
    {
        return _value.ToString().ToLower();
    }

    public override object Clone()
    {
        return new BooleanValue(_value);
    }
}
