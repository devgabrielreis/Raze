using Raze.Script.Core.Constants;

namespace Raze.Script.Core.Runtime.Values;

public class StringValue : RuntimeValue
{
    public override object Value => _value;

    public string StrValue => _value;

    public override string TypeName => TypeNames.STRING_TYPE_NAME;

    private readonly string _value;

    public StringValue(string value)
    {
        _value = value;
    }

    public override string ToString()
    {
        return '"' + _value + '"';
    }

    public override object Clone()
    {
        return new StringValue((_value.Clone() as string)!);
    }
}
