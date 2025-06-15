using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens.Operators;
using Raze.Script.Core.Tokens.Operators.EqualityOperators;

namespace Raze.Script.Core.Values;

public class BooleanValue : RuntimeValue
{
    public override object? Value => _value;

    public bool? BoolValue => _value;

    public override string TypeName => "Boolean";

    private readonly bool? _value;

    public BooleanValue(bool? value)
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
            TypeName,
            other.TypeName,
            op.Lexeme,
            source.StartLine,
            source.StartColumn
        );
    }

    private BooleanValue ExecuteBinaryOperationWithBoolean(OperatorToken op, BooleanValue other, BinaryExpression source)
    {
        if (BoolValue is null || other.BoolValue is null)
        {
            throw new NullValueException(source.StartLine, source.StartColumn);
        }

        switch (op)
        {
            case EqualOperator:
                return new BooleanValue(BoolValue == other.BoolValue);
            case NotEqualOperator:
                return new BooleanValue(BoolValue != other.BoolValue);
            case AndOperator:
                return new BooleanValue(BoolValue.Value && other.BoolValue.Value);
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

        return _value.ToString()!.ToLower();
    }
}
