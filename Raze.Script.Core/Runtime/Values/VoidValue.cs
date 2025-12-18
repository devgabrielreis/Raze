using Raze.Script.Core.Constants;

namespace Raze.Script.Core.Runtime.Values;

public class VoidValue : RuntimeValue
{
    public override object Value => this;

    public override string TypeName => TypeNames.VOID_TYPE_NAME;

    public override string ToString()
    {
        return TypeName;
    }

    public override object Clone()
    {
        return new VoidValue();
    }
}
