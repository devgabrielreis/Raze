using Raze.Script.Core.Constants;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Values;
using Raze.Script.Core.Tokens.Operators;
using Raze.Script.Core.Tokens.Operators.EqualityOperators;

namespace Raze.Script.Core.Runtime.Operations;

internal class BooleanOperationRegistrar : IOperationRegistrar
{
    public static void RegisterBinaryOperations(OperationDispatcher dispatcher)
    {
        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.BOOLEAN_TYPE_NAME, typeof(EqualToken), TypeNames.BOOLEAN_TYPE_NAME),
            EqualBoolean
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.BOOLEAN_TYPE_NAME, typeof(NotEqualToken), TypeNames.BOOLEAN_TYPE_NAME),
            NotEqualBoolean
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.BOOLEAN_TYPE_NAME, typeof(AndToken), TypeNames.BOOLEAN_TYPE_NAME),
            AndBoolean
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.BOOLEAN_TYPE_NAME, typeof(OrToken), TypeNames.BOOLEAN_TYPE_NAME),
            OrBoolean
        );
    }

    public static void RegisterUnaryOperations(OperationDispatcher dispatcher)
    {
        dispatcher.RegisterUnaryOperation(
            new UnaryOperationKey(TypeNames.BOOLEAN_TYPE_NAME, typeof(NotToken), IsPostfix: false),
            Not
        );
    }

    private static RuntimeValue EqualBoolean(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        return new BooleanValue(((BooleanValue)left).BoolValue == ((BooleanValue)right).BoolValue);
    }

    private static RuntimeValue NotEqualBoolean(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        return new BooleanValue(((BooleanValue)left).BoolValue != ((BooleanValue)right).BoolValue);
    }

    private static RuntimeValue AndBoolean(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        return new BooleanValue(((BooleanValue)left).BoolValue && ((BooleanValue)right).BoolValue);
    }

    private static RuntimeValue OrBoolean(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        return new BooleanValue(((BooleanValue)left).BoolValue || ((BooleanValue)right).BoolValue);
    }

    private static RuntimeValue Not(RuntimeValue operand, SourceInfo source)
    {
        bool value = (((BooleanValue)operand).BoolValue);
        value = !value;

        return new BooleanValue(value);
    }
}
