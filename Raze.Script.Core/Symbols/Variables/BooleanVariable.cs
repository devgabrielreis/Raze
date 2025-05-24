using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Symbols.Variables;

public class BooleanVariable : VariableSymbol
{
    public override BooleanValue Value => _value;

    private BooleanValue _value = new(null);

    public BooleanVariable(RuntimeValue value, bool isConstant)
        : base(value, isConstant)
    {
    }

    internal BooleanVariable(RuntimeValue value, bool isConstant, int? sourceLine, int? sourceColumn)
        : base(value, isConstant, sourceLine, sourceColumn)
    {
    }

    protected override void SetValue(RuntimeValue value, int? sourceLine, int? sourceColumn)
    {
        if (value is NullValue)
        {
            _value = new BooleanValue(null);
            return;
        }
        else if (value is BooleanValue boolValue)
        {
            _value = boolValue;
            return;
        }

        throw new VariableTypeException(value.TypeName, nameof(BooleanVariable), sourceLine, sourceColumn);
    }
}
