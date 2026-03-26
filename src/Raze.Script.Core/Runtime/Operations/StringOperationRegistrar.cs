using Raze.Script.Core.Constants;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.Runtime.Operations;

internal sealed class StringOperationRegistrar : IOperationRegistrar
{
    public static void RegisterBinaryOperations(OperationDispatcher dispatcher)
    {
        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.String, Operators.PLUS, RuntimeType.String),
            AddString
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.String, Operators.EQUAL, RuntimeType.String),
            EqualString
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(RuntimeType.String, Operators.NOT_EQUAL, RuntimeType.String),
            NotEqualString
        );
    }

    public static void RegisterUnaryOperations(OperationDispatcher dispatcher)
    {
    }

    private static void AddString(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsString();
        var rightValue = right.AsString();

        result = new RuntimeValue(leftValue + rightValue);
    }

    private static void EqualString(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsString();
        var rightValue = right.AsString();

        var resultValue = leftValue == rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }

    private static void NotEqualString(
        ref readonly RuntimeValue left,
        ref readonly RuntimeValue right,
        out RuntimeValue result,
        ref readonly SourceInfo source
    )
    {
        var leftValue = left.AsString();
        var rightValue = right.AsString();

        var resultValue = leftValue != rightValue;

        result = (resultValue)
                ? RuntimeValue.True
                : RuntimeValue.False;
    }
}
