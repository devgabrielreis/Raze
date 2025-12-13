using Raze.Script.Core.Values;

namespace Raze.Script.Core.Types;

public class BooleanType : RuntimeType
{
    protected override string BaseTypeName => "boolean";

    public BooleanType(bool isNullable)
        : base(isNullable)
    {
    }

    public override bool Equals(RuntimeType? other)
    {
        return other is BooleanType
            && other.IsNullable == IsNullable;
    }

    protected override bool AcceptNonNull(RuntimeValue value)
    {
        return value is BooleanValue;
    }
}
