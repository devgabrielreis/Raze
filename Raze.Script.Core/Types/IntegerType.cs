using Raze.Script.Core.Constants;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Types;

public class IntegerType : RuntimeType
{
    protected override string BaseTypeName => TypeNames.INTEGER_TYPE_NAME;

    public IntegerType(bool isNullable)
        : base(isNullable)
    {
    }

    public override bool Equals(RuntimeType? other)
    {
        return other is IntegerType
            && other.IsNullable == IsNullable;
    }

    protected override bool AcceptNonNullValue(RuntimeValue value)
    {
        return value is IntegerValue;
    }
}
