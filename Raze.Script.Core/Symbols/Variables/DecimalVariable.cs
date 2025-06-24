using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Symbols.Variables;

internal class DecimalVariable : VariableSymbol
{
    public override DecimalValue? Value => _value;

    private DecimalValue? _value = null;

    public override string VariableTypeName => IsNullable ? "NullableDecimalVariable" : "DecimalVariable";

    public DecimalVariable(RuntimeValue? value, bool isConstant, bool isNullable)
        : base(value, isConstant, isNullable)
    {
    }

    internal DecimalVariable(RuntimeValue? value, bool isConstant, bool isNullable, int? sourceLine, int? sourceColumn)
        : base(value, isConstant, isNullable, sourceLine, sourceColumn)
    {
    }

    protected override void SetValue(RuntimeValue value, int? sourceLine, int? sourceColumn)
    {
        if (value is NullValue || value.Value is null)
        {
            _value = new DecimalValue(null);
            return;
        }
        else if (value is DecimalValue decValue)
        {
            _value = decValue;
            return;
        }
        else if (value is IntegerValue intValue)
        {
            _value = new DecimalValue((decimal)intValue.IntValue!);
            return;
        }

        throw new VariableTypeException(value.TypeName, VariableTypeName, sourceLine, sourceColumn);
    }
}
