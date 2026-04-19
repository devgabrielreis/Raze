using System.Runtime.InteropServices;

namespace Raze.Script.Core.Runtime.Values;

[StructLayout(LayoutKind.Explicit, Size = 16)]
internal readonly struct ValueUnion
{
    [FieldOffset(0)] internal readonly Int128 IntegerValue;
    [FieldOffset(0)] internal readonly decimal DecimalValue;
    [FieldOffset(0)] internal readonly bool BoolValue;

    internal ValueUnion(Int128 intValue)
    {
        DecimalValue = default;
        BoolValue = default;

        IntegerValue = intValue;
    }

    internal ValueUnion(decimal decimalValue)
    {
        IntegerValue = default;
        BoolValue = default;

        DecimalValue = decimalValue;
    }

    internal ValueUnion(bool boolValue)
    {
        IntegerValue = default;
        DecimalValue = default;

        BoolValue = boolValue;
    }
}
