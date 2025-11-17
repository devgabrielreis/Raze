using Raze.Script.Core.Values;

namespace Raze.Script.Core.Types;

public class StringType : RuntimeType
{
    protected override string BaseTypeName => "String";

    public StringType(bool isNullable)
        : base(isNullable)
    {
    }

    protected override bool AcceptNonNull(RuntimeValue value)
    {
        return value is StringValue;
    }
}
