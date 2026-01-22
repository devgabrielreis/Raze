using Raze.Script.Core.Constants;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.Runtime.Types;

public class BooleanType : RuntimeType
{
    protected override string BaseTypeName => TypeNames.BOOLEAN_TYPE_NAME;

    public BooleanType(bool isNullable)
        : base(isNullable)
    {
    }

    public override bool Equals(RuntimeType? other)
    {
        return other is BooleanType
            && other.IsNullable == IsNullable;
    }

    protected override bool AcceptNonNullValue(RuntimeValue value)
    {
        return value is BooleanValue;
    }
}
