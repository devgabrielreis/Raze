using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens.Operators;

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
