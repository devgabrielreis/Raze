using Raze.Script.Core.Constants;

namespace Raze.Script.Core.Runtime.Values;

public class IntegerValue : RuntimeValue
{
    public override object Value => _value;

    public Int128 IntValue => _value;

    public override string TypeName => TypeNames.INTEGER_TYPE_NAME;

    private readonly Int128 _value;

    public IntegerValue(Int128 value)
    {
        _value = value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }

    public override object Clone()
    {
        return new IntegerValue(_value);
    }
}
