using Raze.Script.Core.Constants;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.Runtime.Types;

public class VoidType : RuntimeType
{
    protected override string BaseTypeName => TypeNames.VOID_TYPE_NAME;

    public VoidType()
        : base(false)
    {
    }

    public override bool Equals(RuntimeType? other)
    {
        return other is VoidType
            && other.IsNullable == IsNullable;
    }

    protected override bool AcceptNonNullValue(RuntimeValue value)
    {
        return value is VoidValue;
    }
}
