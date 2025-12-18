using Raze.Script.Core.Constants;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.Runtime.Types;

public class StringType : RuntimeType
{
    protected override string BaseTypeName => TypeNames.STRING_TYPE_NAME;

    public StringType(bool isNullable)
        : base(isNullable)
    {
    }

    public override bool Equals(RuntimeType? other)
    {
        return other is StringType
            && other.IsNullable == IsNullable;
    }

    protected override bool AcceptNonNullValue(RuntimeValue value)
    {
        return value is StringValue;
    }
}
