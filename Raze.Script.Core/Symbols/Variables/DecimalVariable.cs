using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Symbols.Variables;

internal class DecimalVariable : VariableSymbol
{
    public DecimalVariable(RuntimeValue value, bool isConstant)
        : base(value, isConstant)
    {
    }

    internal DecimalVariable(RuntimeValue value, bool isConstant, int? sourceLine, int? sourceColumn)
        : base(value, isConstant, sourceLine, sourceColumn)
    {
    }

    protected override void SetValue(RuntimeValue value, int? sourceLine, int? sourceColumn)
    {
        if (value is NullValue || value.Value is null)
        {
            Value = new DecimalValue(null);
            return;
        }
        else if (value is DecimalValue)
        {
            Value = value;
            return;
        }
        else if (value is IntegerValue intValue)
        {
            Value = new DecimalValue((decimal)(int)intValue.Value!);
            return;
        }

        throw new VariableTypeException(value.TypeName, nameof(IntegerVariable), sourceLine, sourceColumn);
    }
}
