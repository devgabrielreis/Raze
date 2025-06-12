﻿using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens.Operators;

namespace Raze.Script.Core.Values;

public class StringValue : RuntimeValue
{
    public override object? Value => _value;

    public string? StrValue => _value;

    public override string TypeName => "String";

    private readonly string? _value;

    public StringValue(string? value)
    {
        _value = value;
    }

    internal override RuntimeValue ExecuteBinaryOperation(OperatorToken op, RuntimeValue other, BinaryExpression source)
    {
        if (other is StringValue otherStr)
        {
            return ExecuteBinaryOperationWithString(op, otherStr, source);
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

        return '"' + _value + '"';
    }

    private RuntimeValue ExecuteBinaryOperationWithString(OperatorToken op, StringValue other, BinaryExpression source)
    {
        if (StrValue is null || other.StrValue is null)
        {
            throw new NullValueException(source.StartLine, source.StartColumn);
        }

        switch (op)
        {
            case AdditionOperator:
                return new StringValue(StrValue + other.StrValue);
            case EqualOperator:
                return new BooleanValue(StrValue == other.StrValue);
            case NotEqualOperator:
                return new BooleanValue(StrValue != other.StrValue);
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
