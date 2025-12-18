using Raze.Script.Core.Constants;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.Runtime.Types;

public class DecimalType : RuntimeType
{
    protected override string BaseTypeName => TypeNames.DECIMAL_TYPE_NAME;

    public DecimalType(bool isNullable)
        : base(isNullable)
    {
    }

    public override bool Equals(RuntimeType? other)
    {
        return other is DecimalType
            && other.IsNullable == IsNullable;
    }

    protected override bool AcceptNonNullValue(RuntimeValue value)
    {
        return value is DecimalValue;
    }
}
