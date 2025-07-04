﻿using Raze.Script.Core.Exceptions.RuntimeExceptions;
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
    public override object? Value => _value;

    public decimal? DecValue => _value;

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

    private RuntimeValue ExecuteBinaryOperationWithDecimal(OperatorToken op, DecimalValue other, BinaryExpression source)
    {
        if (DecValue is null || other.DecValue is null)
        {
            throw new NullValueException(source.StartLine, source.StartColumn);
        }

        switch (op)
        {
            case AdditionOperator:
                return new DecimalValue(DecValue + other.DecValue);
            case SubtractionOperator:
                return new DecimalValue(DecValue - other.DecValue);
            case MultiplicationOperator:
                return new DecimalValue(DecValue * other.DecValue);
            case DivisionOperator:
                if (other.DecValue == 0)
                {
                    throw new DivisionByZeroException(source.StartLine, source.StartColumn);
                }
                return new DecimalValue(DecValue / other.DecValue);
            case ModuloOperator:
                return new DecimalValue(DecValue % other.DecValue);
            case EqualOperator:
                return new BooleanValue(DecValue == other.DecValue);
            case NotEqualOperator:
                return new BooleanValue(DecValue != other.DecValue);
            case GreaterThanOperator:
                return new BooleanValue(DecValue > other.DecValue);
            case LessThanOperator:
                return new BooleanValue(DecValue < other.DecValue);
            case GreaterOrEqualThanOperator:
                return new BooleanValue(DecValue >= other.DecValue);
            case LessOrEqualThanOperator:
                return new BooleanValue(DecValue <= other.DecValue);
        }

        throw new UnsupportedBinaryOperationException(
            TypeName,
            other.TypeName,
            op.Lexeme,
            source.StartLine,
            source.StartColumn
        );
    }

    private RuntimeValue ExecuteBinaryOperationWithInteger(OperatorToken op, IntegerValue other, BinaryExpression source)
    {
        if (DecValue is null || other.IntValue is null)
        {
            throw new NullValueException(source.StartLine, source.StartColumn);
        }

        switch (op)
        {
            case AdditionOperator:
                return new DecimalValue(DecValue + (decimal)other.IntValue);
            case SubtractionOperator:
                return new DecimalValue(DecValue - (decimal)other.IntValue);
            case MultiplicationOperator:
                return new DecimalValue(DecValue * (decimal)other.IntValue);
            case DivisionOperator:
                if (other.IntValue == 0)
                {
                    throw new DivisionByZeroException(source.StartLine, source.StartColumn);
                }
                return new DecimalValue(DecValue / (decimal)other.IntValue);
            case ModuloOperator:
                return new DecimalValue(DecValue % (decimal)other.IntValue);
            case EqualOperator:
                return new BooleanValue(DecValue == (decimal)other.IntValue);
            case NotEqualOperator:
                return new BooleanValue(DecValue != (decimal)other.IntValue);
            case GreaterThanOperator:
                return new BooleanValue(DecValue > (decimal)other.IntValue);
            case LessThanOperator:
                return new BooleanValue(DecValue < (decimal)other.IntValue);
            case GreaterOrEqualThanOperator:
                return new BooleanValue(DecValue >= (decimal)other.IntValue);
            case LessOrEqualThanOperator:
                return new BooleanValue(DecValue <= (decimal)other.IntValue);
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
