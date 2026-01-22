using Raze.Script.Core.Constants;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.Runtime.Operations;

internal class BooleanOperationRegistrar : IOperationRegistrar
{
    public static void RegisterBinaryOperations(OperationDispatcher dispatcher)
    {
        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.BOOLEAN_TYPE_NAME, Operators.EQUAL, TypeNames.BOOLEAN_TYPE_NAME),
            EqualBoolean
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.BOOLEAN_TYPE_NAME, Operators.NOT_EQUAL, TypeNames.BOOLEAN_TYPE_NAME),
            NotEqualBoolean
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.BOOLEAN_TYPE_NAME, Operators.AND, TypeNames.BOOLEAN_TYPE_NAME),
            AndBoolean
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.BOOLEAN_TYPE_NAME, Operators.OR, TypeNames.BOOLEAN_TYPE_NAME),
            OrBoolean
        );
    }

    public static void RegisterUnaryOperations(OperationDispatcher dispatcher)
    {
        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(TypeNames.BOOLEAN_TYPE_NAME, Operators.NOT, IsPostfix: false),
            Not
        );
    }

    private static RuntimeValue EqualBoolean(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((BooleanValue)left).BoolValue;
        var rightValue = ((BooleanValue)right).BoolValue;

        return new BooleanValue(leftValue == rightValue);
    }

    private static RuntimeValue NotEqualBoolean(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((BooleanValue)left).BoolValue;
        var rightValue = ((BooleanValue)right).BoolValue;

        return new BooleanValue(leftValue != rightValue);
    }

    private static RuntimeValue AndBoolean(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((BooleanValue)left).BoolValue;
        var rightValue = ((BooleanValue)right).BoolValue;

        return new BooleanValue(leftValue && rightValue);
    }

    private static RuntimeValue OrBoolean(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        var leftValue = ((BooleanValue)left).BoolValue;
        var rightValue = ((BooleanValue)right).BoolValue;

        return new BooleanValue(leftValue || rightValue);
    }

    private static RuntimeValue Not(RuntimeValue operand, SourceInfo source)
    {
        bool value = (((BooleanValue)operand).BoolValue);
        value = !value;

        return new BooleanValue(value);
    }
}
