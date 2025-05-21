using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Symbols.Variables;

public class IntegerVariable : VariableSymbol
{
    public IntegerVariable(RuntimeValue value, bool isConstant)
        : base(value, isConstant)
    {
    }

    internal IntegerVariable(RuntimeValue value, bool isConstant, int? sourceLine, int? sourceColumn)
        : base(value, isConstant, sourceLine, sourceColumn)
    {
    }

    protected override void SetValue(RuntimeValue value, int? sourceLine, int? sourceColumn)
    {
        if (value is NullValue)
        {
            Value = new IntegerValue(null);
            return;
        }
        else if (value is IntegerValue)
        {
            Value = value;
            return;
        }

        throw new VariableTypeException(value.TypeName, nameof(IntegerVariable), sourceLine, sourceColumn);
    }
}
