using Raze.Script.Core.Constants;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Runtime.Values;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Raze.Script.Core.Runtime.Operations;

internal sealed class IntegerOperationRegistrar : IOperationRegistrar
{
    public static void RegisterBinaryOperations(OperationDispatcher dispatcher)
    {
        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Integer, Operators.PLUS, RuntimeType.Integer),
            AddInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Integer, Operators.MINUS, RuntimeType.Integer),
            SubtractInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Integer, Operators.MULTIPLICATION, RuntimeType.Integer),
            MultiplyInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Integer, Operators.DIVISION, RuntimeType.Integer),
            DivideInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Integer, Operators.MODULO, RuntimeType.Integer),
            ModuloInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Integer, Operators.EQUAL, RuntimeType.Integer),
            EqualInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Integer, Operators.NOT_EQUAL, RuntimeType.Integer),
            NotEqualInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Integer, Operators.LESS_THAN, RuntimeType.Integer),
            LessThanInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Integer, Operators.GREATER_THAN, RuntimeType.Integer),
            GreaterThanInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Integer, Operators.LESS_OR_EQUAL, RuntimeType.Integer),
            LessOrEqualInteger
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Integer, Operators.GREATER_OR_EQUAL, RuntimeType.Integer),
            GreaterOrEqualInteger
        );
    }

    public static void RegisterUnaryOperations(OperationDispatcher dispatcher)
    {
        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(RuntimeType.Integer, Operators.PLUS, IsPostfix: false),
            UnaryPlus
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(RuntimeType.Integer, Operators.MINUS, IsPostfix: false),
            UnaryMinus
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(RuntimeType.Integer, Operators.INCREMENT, IsPostfix: true),
            Increment
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(RuntimeType.Integer, Operators.INCREMENT, IsPostfix: false),
            Increment
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(RuntimeType.Integer, Operators.DECREMENT, IsPostfix: true),
            Decrement
        );

        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(RuntimeType.Integer, Operators.DECREMENT, IsPostfix: false),
            Decrement
        );
    }

    private static void AddInteger(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsInteger();
        var rightValue = right.AsInteger();

        var resultValue = leftValue + rightValue;

        result = new RuntimeValue(resultValue);
    }

    private static void SubtractInteger(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsInteger();
        var rightValue = right.AsInteger();

        var resultValue = leftValue - rightValue;

        result = new RuntimeValue(resultValue);
    }

    private static void MultiplyInteger(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsInteger();
        var rightValue = right.AsInteger();

        var resultValue = leftValue * rightValue;

        result = new RuntimeValue(resultValue);
    }

    private static void DivideInteger(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsInteger();
        var rightValue = right.AsInteger();

        if (rightValue == 0)
        {
            ThrowDivisionByZeroException(in source);
        }

        var resultValue = leftValue / rightValue;

        result = new RuntimeValue(resultValue);
    }

    private static void ModuloInteger(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsInteger();
        var rightValue = right.AsInteger();

        if (rightValue == 0)
        {
            ThrowDivisionByZeroException(in source);
        }

        var resultValue = leftValue % rightValue;

        result = new RuntimeValue(resultValue);
    }

    private static void EqualInteger(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsInteger();
        var rightValue = right.AsInteger();

        var resultValue = leftValue == rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }

    private static void NotEqualInteger(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsInteger();
        var rightValue = right.AsInteger();

        var resultValue = leftValue != rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }

    private static void LessThanInteger(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsInteger();
        var rightValue = right.AsInteger();

        var resultValue = leftValue < rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }

    private static void GreaterThanInteger(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsInteger();
        var rightValue = right.AsInteger();

        var resultValue = leftValue > rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }

    private static void LessOrEqualInteger(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsInteger();
        var rightValue = right.AsInteger();

        var resultValue = leftValue <= rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }

    private static void GreaterOrEqualInteger(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsInteger();
        var rightValue = right.AsInteger();

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
        var value = operand.AsInteger();

        result = new RuntimeValue(value);
    }

    private static void UnaryMinus(
        ref readonly RuntimeValue operand,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var value = operand.AsInteger();
        value = -value;

        result = new RuntimeValue(value);
    }

    private static void Increment(
        ref readonly RuntimeValue operand,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var value = operand.AsInteger();
        value++;

        result = new RuntimeValue(value);
    }

    private static void Decrement(
        ref readonly RuntimeValue operand,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var value = operand.AsInteger();
        value--;

        result = new RuntimeValue(value);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    [DoesNotReturn]
    [StackTraceHidden]
    private static void ThrowDivisionByZeroException(ref readonly SourceInfo source)
    {
        throw new DivisionByZeroException(source);
    }
}
