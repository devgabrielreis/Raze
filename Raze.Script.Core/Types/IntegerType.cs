using Raze.Script.Core.Values;

namespace Raze.Script.Core.Types;

public class IntegerType : RuntimeType
{
    protected override string BaseTypeName => "Integer";

    public IntegerType(bool isNullable)
        : base(isNullable)
    {
    }

    protected override bool AcceptNonNull(RuntimeValue value)
    {
        return value is IntegerValue;
    }
}
