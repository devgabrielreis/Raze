namespace Raze.Script.Core.Runtime.Values;

public sealed class RazeFunctionReturnValue
{
    public static RazeFunctionReturnValue Null => new(in RuntimeValue.Null);
    public static RazeFunctionReturnValue Void => new(in RuntimeValue.Void);

    internal readonly RuntimeValue Value;

    private RazeFunctionReturnValue(ref readonly RuntimeValue value)
    {
        Value = value;
    }

    public static RazeFunctionReturnValue FromValue<T>(T value)
    {
        RuntimeValueUtils.ValueToRuntimeValue(value, out var runtimeValue);

        return new RazeFunctionReturnValue(in runtimeValue);
    }
}
