using Raze.Script.Core.Constants;

namespace Raze.Script.Core.Runtime.Values;

public class BooleanValue : RuntimeValue
{
    public override object Value => _value;

    public bool BoolValue => _value;

    public override string TypeName => TypeNames.BOOLEAN_TYPE_NAME;

    private readonly bool _value;

    public BooleanValue(bool value)
    {
        _value = value;
    }

    public override string ToString()
    {
        return _value.ToString().ToLower();
    }

    public override object Clone()
    {
        return new BooleanValue(_value);
    }
}
