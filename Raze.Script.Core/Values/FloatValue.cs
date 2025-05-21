using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens.Operators;
using System.Globalization;

namespace Raze.Script.Core.Values;

public class FloatValue : RuntimeValue
{
    public override object? Value => _value;

    public override string TypeName => "Float";

    private readonly float? _value;

    public FloatValue(float? value)
    {
        _value = value;
    }

    internal override RuntimeValue ExecuteBinaryOperation(OperatorToken op, RuntimeValue other, BinaryExpression source)
    {
        if (other is FloatValue floatValue)
        {
            return ExecuteBinaryOperationWithFloat(op, floatValue, source);
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

        string floatStr = ((float)_value).ToString(CultureInfo.InvariantCulture);

        if (!floatStr.Contains('.'))
        {
            floatStr += ".0";
        }

        return floatStr;
    }

    private FloatValue ExecuteBinaryOperationWithFloat(OperatorToken op, FloatValue other, BinaryExpression source)
    {
        if (Value is null || other.Value is null)
        {
            throw new NullValueException(source.StartLine, source.StartColumn);
        }

        switch (op)
        {
            case AdditionOperator:
                return new FloatValue((float)Value + (float)other.Value);
            case SubtractionOperator:
                return new FloatValue((float)Value - (float)other.Value);
            case MultiplicationOperator:
                return new FloatValue((float)Value * (float)other.Value);
            case DivisionOperator:
                if ((float)other.Value == 0)
                {
                    throw new DivisionByZeroException(source.StartLine, source.StartColumn);
                }
                return new FloatValue((float)Value / (float)other.Value);
            case ModuloOperator:
                return new FloatValue((float)Value % (float)other.Value);
        }

        throw new UnsupportedBinaryOperationException(
            TypeName,
            other.TypeName,
            op.Lexeme,
            source.StartLine,
            source.StartColumn
        );
    }

    private FloatValue ExecuteBinaryOperationWithInteger(OperatorToken op, IntegerValue other, BinaryExpression source)
    {
        if (Value is null || other.Value is null)
        {
            throw new NullValueException(source.StartLine, source.StartColumn);
        }

        switch (op)
        {
            case AdditionOperator:
                return new FloatValue((float)Value + (float)(int)other.Value);
            case SubtractionOperator:
                return new FloatValue((float)Value - (float)(int)other.Value);
            case MultiplicationOperator:
                return new FloatValue((float)Value * (float)(int)other.Value);
            case DivisionOperator:
                if ((int)other.Value == 0)
                {
                    throw new DivisionByZeroException(source.StartLine, source.StartColumn);
                }
                return new FloatValue((float)Value / (float)(int)other.Value);
            case ModuloOperator:
                return new FloatValue((float)Value % (float)(int)other.Value);
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
