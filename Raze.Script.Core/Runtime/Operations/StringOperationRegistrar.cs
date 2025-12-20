using Raze.Script.Core.Constants;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Values;
using Raze.Script.Core.Tokens.Operators.AdditiveOperators;
using Raze.Script.Core.Tokens.Operators.EqualityOperators;

namespace Raze.Script.Core.Runtime.Operations;

internal class StringOperationRegistrar : IOperationRegistrar
{
    public static void RegisterBinaryOperations(OperationDispatcher dispatcher)
    {
        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.STRING_TYPE_NAME, typeof(AdditionToken), TypeNames.STRING_TYPE_NAME),
            AddString
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.STRING_TYPE_NAME, typeof(EqualToken), TypeNames.STRING_TYPE_NAME),
            EqualString
        );

        dispatcher.RegisterBinaryOperation(
            new BinaryOperationKey(TypeNames.STRING_TYPE_NAME, typeof(NotEqualToken), TypeNames.STRING_TYPE_NAME),
            NotEqualString
        );
    }

    public static void RegisterUnaryOperations(OperationDispatcher dispatcher)
    {
    }

    private static RuntimeValue AddString(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        return new StringValue(((StringValue)left).StrValue + ((StringValue)right).StrValue);
    }

    private static RuntimeValue EqualString(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        return new BooleanValue(((StringValue)left).StrValue == ((StringValue)right).StrValue);
    }

    private static RuntimeValue NotEqualString(RuntimeValue left, RuntimeValue right, SourceInfo source)
    {
        return new BooleanValue(((StringValue)left).StrValue != ((StringValue)right).StrValue);
    }
}
