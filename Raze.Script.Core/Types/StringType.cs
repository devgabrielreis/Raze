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
        return other is StringType
            && other.IsNullable == IsNullable;
    }

    protected override bool AcceptNonNullValue(RuntimeValue value)
    {
        return value is StringValue;
    }
}
