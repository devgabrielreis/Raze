using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.Runtime.Context;

internal readonly struct ReturnedValue
{
    internal readonly RuntimeValue Value;
    internal readonly SourceInfo Source;

    internal ReturnedValue(
        ref readonly RuntimeValue value,
        ref readonly SourceInfo source
    )
    {
        Value = value;
        Source = source;
    }
}