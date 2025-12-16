using Raze.Script.Core.Values;

namespace Raze.Script.Core.Types;

public class VoidType : RuntimeType
{
    protected override string BaseTypeName => "void";

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
