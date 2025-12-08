using Raze.Script.Core.Values;

namespace Raze.Script.Core.Types;

public class IntegerType : RuntimeType
{
    protected override string BaseTypeName => "Integer";

    public IntegerType(bool isNullable)
        : base(isNullable)
    {
    }

    public override bool Equals(RuntimeType? other)
    {
        return other is IntegerType;
    }

    protected override bool AcceptNonNull(RuntimeValue value)
    {
        return value is IntegerValue;
    }
}
