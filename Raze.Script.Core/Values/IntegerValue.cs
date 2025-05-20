using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens.Operators;

namespace Raze.Script.Core.Values;

public class IntegerValue : RuntimeValue
{
    public override object? Value => _value;

    public override string TypeName => "Integer";

    private readonly int? _value;

    public IntegerValue(int? value)
    {
        _value = value;
    }

    internal override RuntimeValue ExecuteBinaryOperation(OperatorToken op, RuntimeValue other, BinaryExpression source)
    {
        if (other.TypeName != TypeName)
        {
            throw new UnsupportedBinaryOperationException(
                TypeName,
                other.TypeName,
                op.Lexeme,
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
            case AdditionOperator:
                return new IntegerValue((int)Value + (int)other.Value);
            case SubtractionOperator:
                return new IntegerValue((int)Value - (int)other.Value);
            case MultiplicationOperator:
                return new IntegerValue((int)Value * (int)other.Value);
            case DivisionOperator:
                if ((int)other.Value == 0)
                {
                    throw new DivisionByZeroException(source.StartLine, source.StartColumn);
                }
                return new IntegerValue((int)Value / (int)other.Value);
            case ModuloOperator:
                return new IntegerValue((int)Value % (int)other.Value);
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

        return _value.ToString()!;
    }
}
