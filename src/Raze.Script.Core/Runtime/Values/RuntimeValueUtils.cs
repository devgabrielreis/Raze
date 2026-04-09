using Raze.Script.Core.Exceptions;

namespace Raze.Script.Core.Runtime.Values;

internal static class RuntimeValueUtils
{
    internal static void ValueToRuntimeValue<T>(T value, out RuntimeValue runtimeValue)
    {
        runtimeValue = value switch
        {
            Int128 intValue  => new RuntimeValue(intValue),
            decimal decValue => new RuntimeValue(decValue),
            string strValue  => new RuntimeValue(strValue),
            bool boolValue   => boolValue ? RuntimeValue.True : RuntimeValue.False,
            _ => ThrowHelper.ThrowInvalidOperationException<RuntimeValue>(
                "The type of the value must be one of: Int128, decimal, bool, string"
            )
        };
    }
}
