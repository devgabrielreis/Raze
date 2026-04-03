using Raze.Script.Core.Constants;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.Runtime.Operations;

internal sealed class BooleanOperationRegistrar : IOperationRegistrar
{
    public static void RegisterBinaryOperations(OperationDispatcher dispatcher)
    {
        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Boolean, Operators.EQUAL, RuntimeType.Boolean),
            EqualBoolean
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Boolean, Operators.NOT_EQUAL, RuntimeType.Boolean),
            NotEqualBoolean
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Boolean, Operators.AND, RuntimeType.Boolean),
            AndBoolean
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.Boolean, Operators.OR, RuntimeType.Boolean),
            OrBoolean
        );
    }

    public static void RegisterUnaryOperations(OperationDispatcher dispatcher)
    {
        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(RuntimeType.Boolean, Operators.NOT, IsPostfix: false),
            Not
        );
    }

    private static void EqualBoolean(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsBoolean();
        var rightValue = right.AsBoolean();

        var resultValue = leftValue == rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }

    private static void NotEqualBoolean(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsBoolean();
        var rightValue = right.AsBoolean();

        var resultValue = leftValue != rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }

    private static void AndBoolean(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsBoolean();
        var rightValue = right.AsBoolean();

        var resultValue = leftValue && rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }

    private static void OrBoolean(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsBoolean();
        var rightValue = right.AsBoolean();

        var resultValue = leftValue || rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }

    private static void Not(
        ref readonly RuntimeValue operand,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        bool value = operand.AsBoolean();
        value = !value;

        result = (value)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }
}
