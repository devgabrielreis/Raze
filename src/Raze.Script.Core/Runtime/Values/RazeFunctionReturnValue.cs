namespace Raze.Script.Core.Runtime.Values;

internal sealed class RazeFunctionReturnValue
{
    internal static RazeFunctionReturnValue Null => new(in RuntimeValue.Null);
    internal static RazeFunctionReturnValue Void => new(in RuntimeValue.Void);

    internal readonly RuntimeValue Value;

    private RazeFunctionReturnValue(ref readonly RuntimeValue value)
    {
        Value = value;
    }

    internal static RazeFunctionReturnValue FromValue<T>(T value)
    {
        RuntimeValueUtils.ValueToRuntimeValue(value, out var runtimeValue);

        return new RazeFunctionReturnValue(in runtimeValue);
    }
}
