using Raze.Script.Core.Values;

namespace Raze.Script.Core.Types;

public class StringType : RuntimeType
{
    protected override string BaseTypeName => "string";

    public StringType(bool isNullable)
        : base(isNullable)
    {
    }

    public override bool Equals(RuntimeType? other)
    {
        return other is StringType;
    }

    protected override bool AcceptNonNull(RuntimeValue value)
    {
        return value is StringValue;
    }
}
