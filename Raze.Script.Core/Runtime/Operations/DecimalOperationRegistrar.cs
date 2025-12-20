using Raze.Script.Core.Constants;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Values;
using Raze.Script.Core.Tokens.Operators;
using Raze.Script.Core.Tokens.Operators.AdditiveOperators;
using Raze.Script.Core.Tokens.Operators.EqualityOperators;
using Raze.Script.Core.Tokens.Operators.MultiplicativeOperators;
using Raze.Script.Core.Tokens.Operators.RelationalOperators;

namespace Raze.Script.Core.Runtime.Operations;

internal class DecimalOperationRegistrar : IOperationRegistrar
{
    public static void RegisterBinaryOperations(OperationDispatcher dispatcher)
    {
        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(AdditionToken), TypeNames.DECIMAL_TYPE_NAME),
            AddDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(SubtractionToken), TypeNames.DECIMAL_TYPE_NAME),
            SubtractDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(MultiplicationToken), TypeNames.DECIMAL_TYPE_NAME),
            MultiplyDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(DivisionToken), TypeNames.DECIMAL_TYPE_NAME),
            DivideDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(ModuloToken), TypeNames.DECIMAL_TYPE_NAME),
            ModuloDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(EqualToken), TypeNames.DECIMAL_TYPE_NAME),
            EqualDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(NotEqualToken), TypeNames.DECIMAL_TYPE_NAME),
            NotEqualDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(LessThanToken), TypeNames.DECIMAL_TYPE_NAME),
            LessThanDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(GreaterThanToken), TypeNames.DECIMAL_TYPE_NAME),
            GreaterThanDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(LessOrEqualThanToken), TypeNames.DECIMAL_TYPE_NAME),
            LessOrEqualDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(GreaterOrEqualThanToken), TypeNames.DECIMAL_TYPE_NAME),
            GreaterOrEqualDecimal
        );
    }

    public static void RegisterUnaryOperations(OperationDispatcher dispatcher)
    {
        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(AdditionToken), IsPostfix: false),
            UnaryPlus
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(SubtractionToken), IsPostfix: false),
            UnaryMinus
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(IncrementToken), IsPostfix: true),
            Increment
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(IncrementToken), IsPostfix: false),
            Increment
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(DecrementToken), IsPostfix: true),
            Decrement
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(TypeNames.DECIMAL_TYPE_NAME, typeof(DecrementToken), IsPostfix: false),
            Decrement
        );
    }

    private static RuntimeValue AddDecimal(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((DecimalValue)left).DecValue;
        var rightValue = ((DecimalValue)right).DecValue;

        return new DecimalValue(leftValue + rightValue);
    }

    private static RuntimeValue SubtractDecimal(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((DecimalValue)left).DecValue;
        var rightValue = ((DecimalValue)right).DecValue;

        return new DecimalValue(leftValue - rightValue);
    }

    private static RuntimeValue MultiplyDecimal(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((DecimalValue)left).DecValue;
        var rightValue = ((DecimalValue)right).DecValue;

        return new DecimalValue(leftValue * rightValue);
    }

    private static RuntimeValue DivideDecimal(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((DecimalValue)left).DecValue;
        var rightValue = ((DecimalValue)right).DecValue;

        if (rightValue == 0.0m)
        {
            throw new DivisionByZeroException(source);
        }

        return new DecimalValue(leftValue / rightValue);
    }

    private static RuntimeValue ModuloDecimal(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((DecimalValue)left).DecValue;
        var rightValue = ((DecimalValue)right).DecValue;

        if (rightValue == 0.0m)
        {
            throw new DivisionByZeroException(source);
        }

        return new DecimalValue(leftValue % rightValue);
    }

    private static RuntimeValue EqualDecimal(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((DecimalValue)left).DecValue;
        var rightValue = ((DecimalValue)right).DecValue;

        return new BooleanValue(leftValue == rightValue);
    }

    private static RuntimeValue NotEqualDecimal(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((DecimalValue)left).DecValue;
        var rightValue = ((DecimalValue)right).DecValue;

        return new BooleanValue(leftValue != rightValue);
    }

    private static RuntimeValue LessThanDecimal(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((DecimalValue)left).DecValue;
        var rightValue = ((DecimalValue)right).DecValue;

        return new BooleanValue(leftValue < rightValue);
    }

    private static RuntimeValue GreaterThanDecimal(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((DecimalValue)left).DecValue;
        var rightValue = ((DecimalValue)right).DecValue;

        return new BooleanValue(leftValue > rightValue);
    }

    private static RuntimeValue LessOrEqualDecimal(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((DecimalValue)left).DecValue;
        var rightValue = ((DecimalValue)right).DecValue;

        return new BooleanValue(leftValue <= rightValue);
    }

    private static RuntimeValue GreaterOrEqualDecimal(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((DecimalValue)left).DecValue;
        var rightValue = ((DecimalValue)right).DecValue;

        return new BooleanValue(leftValue >= rightValue);
    }

    private static RuntimeValue UnaryPlus(RuntimeValue operand, SourceInfo source)
    {
        var value = ((DecimalValue)operand).DecValue;

        return new DecimalValue(value);
    }

    private static RuntimeValue UnaryMinus(RuntimeValue operand, SourceInfo source)
    {
        var value = ((DecimalValue)operand).DecValue;
        value = -value;

        return new DecimalValue(value);
    }

    private static RuntimeValue Increment(RuntimeValue operand, SourceInfo source)
    {
        var value = ((DecimalValue)operand).DecValue;
        value++;

        return new DecimalValue(value);
    }

    private static RuntimeValue Decrement(RuntimeValue operand, SourceInfo source)
    {
        var value = ((DecimalValue)operand).DecValue;
        value--;

        return new DecimalValue(value);
    }
}
