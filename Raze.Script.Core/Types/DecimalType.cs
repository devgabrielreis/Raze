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
        return other is DecimalType;
    }

    protected override bool AcceptNonNull(RuntimeValue value)
    {
        return value is DecimalValue;
    }
}
