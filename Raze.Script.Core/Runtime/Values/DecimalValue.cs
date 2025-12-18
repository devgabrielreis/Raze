using Raze.Script.Core.Constants;
using System.Globalization;

namespace Raze.Script.Core.Runtime.Values;

public class DecimalValue : RuntimeValue
{
    public override object Value => _value;

    public decimal DecValue => _value;

    public override string TypeName => TypeNames.DECIMAL_TYPE_NAME;

    private readonly decimal _value;

    public DecimalValue(decimal value)
    {
        _value = value;
    }

    public override string ToString()
    {
        string decimalStr = ((decimal)_value).ToString(CultureInfo.InvariantCulture);

        if (!decimalStr.Contains('.'))
        {
            decimalStr += ".0";
        }

        return decimalStr;
    }

    public override object Clone()
    {
        return new DecimalValue(_value);
    }
}
