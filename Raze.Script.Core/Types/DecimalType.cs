using Raze.Script.Core.Values;

namespace Raze.Script.Core.Types;

public class DecimalType : RuntimeType
{
    protected override string BaseTypeName => "Decimal";

    public DecimalType(bool isNullable)
        : base(isNullable)
    {
    }

    protected override bool AcceptNonNull(RuntimeValue value)
    {
        return value is DecimalValue;
    }
}
