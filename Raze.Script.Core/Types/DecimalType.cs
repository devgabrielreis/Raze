using Raze.Script.Core.Values;

namespace Raze.Script.Core.Types;

public class DecimalType : RuntimeType
{
    protected override string BaseTypeName => "decimal";

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
