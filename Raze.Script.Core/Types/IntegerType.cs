using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Types;

public class IntegerType : RuntimeType
{
    public override object? Value => _value;

    public override string TypeName => "Integer";

    private readonly int? _value;

    public IntegerType(int? value)
    {
        _value = value;
    }

    public override RuntimeType ExecuteBinaryOperation(string op, RuntimeType other, BinaryExpression source)
    {
        if (other.TypeName != TypeName)
        {
            throw new UnsupportedBinaryOperationException(
                TypeName,
                other.TypeName,
                op,
                source.StartLine,
                source.StartColumn
            );
        }

        if (Value is null || other.Value is null)
        {
            throw new NullValueException(source.StartLine, source.StartColumn);
        }

        switch (op)
        {
            case "+":
                return new IntegerType((int)Value + (int)other.Value);
            case "-":
                return new IntegerType((int)Value - (int)other.Value);
            case "*":
                return new IntegerType((int)Value * (int)other.Value);
            case "/":
                return new IntegerType((int)Value / (int)other.Value);
            case "%":
                return new IntegerType((int)Value % (int)other.Value);
        }

        throw new UnsupportedBinaryOperationException(
            TypeName,
            other.TypeName,
            op,
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

        return _value.ToString()!;
    }
}
