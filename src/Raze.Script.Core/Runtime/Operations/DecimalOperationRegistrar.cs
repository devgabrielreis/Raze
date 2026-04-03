using Raze.Script.Core.Constants;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Runtime.Values;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Raze.Script.Core.Runtime.Operations;

internal sealed class DecimalOperationRegistrar : IOperationRegistrar
{
    public static void RegisterBinaryOperations(OperationDispatcher dispatcher)
    {
        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Decimal, Operators.PLUS, RuntimeType.Decimal),
            AddDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Decimal, Operators.MINUS, RuntimeType.Decimal),
            SubtractDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Decimal, Operators.MULTIPLICATION, RuntimeType.Decimal),
            MultiplyDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Decimal, Operators.DIVISION, RuntimeType.Decimal),
            DivideDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Decimal, Operators.MODULO, RuntimeType.Decimal),
            ModuloDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Decimal, Operators.EQUAL, RuntimeType.Decimal),
            EqualDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Decimal, Operators.NOT_EQUAL, RuntimeType.Decimal),
            NotEqualDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Decimal, Operators.LESS_THAN, RuntimeType.Decimal),
            LessThanDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Decimal, Operators.GREATER_THAN, RuntimeType.Decimal),
            GreaterThanDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Decimal, Operators.LESS_OR_EQUAL, RuntimeType.Decimal),
            LessOrEqualDecimal
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Decimal, Operators.GREATER_OR_EQUAL, RuntimeType.Decimal),
            GreaterOrEqualDecimal
        );
    }

    public static void RegisterUnaryOperations(OperationDispatcher dispatcher)
    {
        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(RuntimeType.Decimal, Operators.PLUS, IsPostfix: false),
            UnaryPlus
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(RuntimeType.Decimal, Operators.MINUS, IsPostfix: false),
            UnaryMinus
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(RuntimeType.Decimal, Operators.INCREMENT, IsPostfix: true),
            Increment
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(RuntimeType.Decimal, Operators.INCREMENT, IsPostfix: false),
            Increment
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(RuntimeType.Decimal, Operators.DECREMENT, IsPostfix: true),
            Decrement
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(RuntimeType.Decimal, Operators.DECREMENT, IsPostfix: false),
            Decrement
        );
    }

    private static void AddDecimal(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsDecimal();
        var rightValue = right.AsDecimal();

        result = new RuntimeValue(leftValue + rightValue);
    }

    private static void SubtractDecimal(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsDecimal();
        var rightValue = right.AsDecimal();

        result = new RuntimeValue(leftValue - rightValue);
    }

    private static void MultiplyDecimal(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsDecimal();
        var rightValue = right.AsDecimal();

        result = new RuntimeValue(leftValue * rightValue);
    }

    private static void DivideDecimal(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsDecimal();
        var rightValue = right.AsDecimal();

        if (rightValue == 0.0m)
        {
            ThrowDivisionByZeroException(in source);
        }

        result = new RuntimeValue(leftValue / rightValue);
    }

    private static void ModuloDecimal(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsDecimal();
        var rightValue = right.AsDecimal();

        if (rightValue == 0.0m)
        {
            ThrowDivisionByZeroException(in source);
        }

        result = new RuntimeValue(leftValue % rightValue);
    }

    private static void EqualDecimal(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsDecimal();
        var rightValue = right.AsDecimal();

        var resultValue = leftValue == rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }

    private static void NotEqualDecimal(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsDecimal();
        var rightValue = right.AsDecimal();

        var resultValue = leftValue != rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }

    private static void LessThanDecimal(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsDecimal();
        var rightValue = right.AsDecimal();

        var resultValue = leftValue < rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }

    private static void GreaterThanDecimal(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsDecimal();
        var rightValue = right.AsDecimal();

        var resultValue = leftValue > rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }

    private static void LessOrEqualDecimal(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsDecimal();
        var rightValue = right.AsDecimal();

        var resultValue = leftValue <= rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }

    private static void GreaterOrEqualDecimal(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsDecimal();
        var rightValue = right.AsDecimal();

        var resultValue = leftValue >= rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }

    private static void UnaryPlus(
        ref readonly RuntimeValue operand,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var value = operand.AsDecimal();

        result = new RuntimeValue(value);
    }

    private static void UnaryMinus(
        ref readonly RuntimeValue operand,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var value = operand.AsDecimal();
        value = -value;

        result = new RuntimeValue(value);
    }

    private static void Increment(
        ref readonly RuntimeValue operand,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var value = operand.AsDecimal();
        value++;

        result = new RuntimeValue(value);
    }

    private static void Decrement(
        ref readonly RuntimeValue operand,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var value = operand.AsDecimal();
        value--;

        result = new RuntimeValue(value);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    [DoesNotReturn]
    [StackTraceHidden]
    private static void ThrowDivisionByZeroException(ref readonly SourceInfo source)
    {
        throw new DivisionByZeroException("Cannot divide by 0", source);
    }
}
