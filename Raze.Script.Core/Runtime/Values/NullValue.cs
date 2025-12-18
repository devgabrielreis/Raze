using Raze.Script.Core.Constants;

namespace Raze.Script.Core.Runtime.Values;

public class NullValue : RuntimeValue
{
    public override object Value => this;

    public override string TypeName => TypeNames.NULL_TYPE_NAME;

    public override string ToString()
    {
        return TypeName;
    }

    public override object Clone()
    {
        return new NullValue();
    }
}
