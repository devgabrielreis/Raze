using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Symbols.Variables;

public class StringVariable : VariableSymbol
{
    public StringVariable(RuntimeValue value, bool isConstant)
        : base(value, isConstant)
    {
    }

    internal StringVariable(RuntimeValue value, bool isConstant, int? sourceLine, int? sourceColumn)
        : base(value, isConstant, sourceLine, sourceColumn)
    {
    }

    protected override void SetValue(RuntimeValue value, int? sourceLine, int? sourceColumn)
    {
        if (value is NullValue)
        {
            Value = new StringValue(null);
            return;
        }
        else if (value is StringValue)
        {
            Value = value;
            return;
        }

        throw new VariableTypeException(value.TypeName, nameof(StringValue), sourceLine, sourceColumn);
    }
}
