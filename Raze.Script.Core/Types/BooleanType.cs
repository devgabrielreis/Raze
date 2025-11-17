using Raze.Script.Core.Values;

namespace Raze.Script.Core.Types;

public class BooleanType : RuntimeType
{
    protected override string BaseTypeName => "Boolean";

    public BooleanType(bool isNullable)
        : base(isNullable)
    {
    }

    protected override bool AcceptNonNull(RuntimeValue value)
    {
        return value is BooleanValue;
    }
}
