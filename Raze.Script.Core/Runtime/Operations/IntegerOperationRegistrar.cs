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

internal class IntegerOperationRegistrar : IOperationRegistrar
{
    public static void RegisterBinaryOperations(OperationDispatcher dispatcher)
    {
        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(AdditionToken), TypeNames.INTEGER_TYPE_NAME),
            AddInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(SubtractionToken), TypeNames.INTEGER_TYPE_NAME),
            SubtractInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(MultiplicationToken), TypeNames.INTEGER_TYPE_NAME),
            MultiplyInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(DivisionToken), TypeNames.INTEGER_TYPE_NAME),
            DivideInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(ModuloToken), TypeNames.INTEGER_TYPE_NAME),
            ModuloInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(EqualToken), TypeNames.INTEGER_TYPE_NAME),
            EqualInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(NotEqualToken), TypeNames.INTEGER_TYPE_NAME),
            NotEqualInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(LessThanToken), TypeNames.INTEGER_TYPE_NAME),
            LessThanInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(GreaterThanToken), TypeNames.INTEGER_TYPE_NAME),
            GreaterThanInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(LessOrEqualThanToken), TypeNames.INTEGER_TYPE_NAME),
            LessOrEqualInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(GreaterOrEqualThanToken), TypeNames.INTEGER_TYPE_NAME),
            GreaterOrEqualInteger
        );
    }

    public static void RegisterUnaryOperations(OperationDispatcher dispatcher)
    {
        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(AdditionToken), IsPostfix: false),
            UnaryPlus
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(SubtractionToken), IsPostfix: false),
            UnaryMinus
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(IncrementToken), IsPostfix: true),
            Increment
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(IncrementToken), IsPostfix: false),
            Increment
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(DecrementToken), IsPostfix: true),
            Decrement
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(TypeNames.INTEGER_TYPE_NAME, typeof(DecrementToken), IsPostfix: false),
            Decrement
        );
    }

    private static RuntimeValue AddInteger(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        return new IntegerValue(((IntegerValue)left).IntValue + ((IntegerValue)right).IntValue);
    }

    private static RuntimeValue SubtractInteger(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((IntegerValue)left).IntValue;
        var rightValue = ((IntegerValue)right).IntValue;

        return new IntegerValue(leftValue - rightValue);
    }

    private static RuntimeValue MultiplyInteger(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((IntegerValue)left).IntValue;
        var rightValue = ((IntegerValue)right).IntValue;

        return new IntegerValue(leftValue * rightValue);
    }

    private static RuntimeValue DivideInteger(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((IntegerValue)left).IntValue;
        var rightValue = ((IntegerValue)right).IntValue;

        if (rightValue == 0)
        {
            throw new DivisionByZeroException(source);
        }

        return new IntegerValue(leftValue / rightValue);
    }

    private static RuntimeValue ModuloInteger(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((IntegerValue)left).IntValue;
        var rightValue = ((IntegerValue)right).IntValue;

        if (rightValue == 0)
        {
            throw new DivisionByZeroException(source);
        }

        return new IntegerValue(leftValue % rightValue);
    }

    private static RuntimeValue EqualInteger(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((IntegerValue)left).IntValue;
        var rightValue = ((IntegerValue)right).IntValue;

        return new BooleanValue(leftValue == rightValue);
    }

    private static RuntimeValue NotEqualInteger(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((IntegerValue)left).IntValue;
        var rightValue = ((IntegerValue)right).IntValue;

        return new BooleanValue(leftValue != rightValue);
    }

    private static RuntimeValue LessThanInteger(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((IntegerValue)left).IntValue;
        var rightValue = ((IntegerValue)right).IntValue;

        return new BooleanValue(leftValue < rightValue);
    }

    private static RuntimeValue GreaterThanInteger(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((IntegerValue)left).IntValue;
        var rightValue = ((IntegerValue)right).IntValue;

        return new BooleanValue(leftValue > rightValue);
    }

    private static RuntimeValue LessOrEqualInteger(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((IntegerValue)left).IntValue;
        var rightValue = ((IntegerValue)right).IntValue;

        return new BooleanValue(leftValue <= rightValue);
    }

    private static RuntimeValue GreaterOrEqualInteger(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((IntegerValue)left).IntValue;
        var rightValue = ((IntegerValue)right).IntValue;

        return new BooleanValue(leftValue >= rightValue);
    }

    private static RuntimeValue UnaryPlus(RuntimeValue operand, SourceInfo source)
    {
        var value = ((IntegerValue)operand).IntValue;

        return new IntegerValue(value);
    }

    private static RuntimeValue UnaryMinus(RuntimeValue operand, SourceInfo source)
    {
        var value = ((IntegerValue)operand).IntValue;
        value = -value;

        return new IntegerValue(value);
    }

    private static RuntimeValue Increment(RuntimeValue operand, SourceInfo source)
    {
        var value = ((IntegerValue)operand).IntValue;
        value++;

        return new IntegerValue(value);
    }

    private static RuntimeValue Decrement(RuntimeValue operand, SourceInfo source)
    {
        var value = ((IntegerValue)operand).IntValue;
        value--;

        return new IntegerValue(value);
    }
}
