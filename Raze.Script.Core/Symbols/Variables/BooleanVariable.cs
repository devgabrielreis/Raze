using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Symbols.Variables;

public class BooleanVariable : VariableSymbol
{
    public override BooleanValue? Value => _value;

    private BooleanValue _value = new(null);

    public override string VariableTypeName => IsNullable ? "NullableBooleanVariable" : "BooleanVariable";

    public BooleanVariable(RuntimeValue? value, bool isConstant, bool isNullable)
        : base(value, isConstant, isNullable)
    {
    }

    internal BooleanVariable(RuntimeValue? value, bool isConstant, bool isNullable, int? sourceLine, int? sourceColumn)
        : base(value, isConstant, isNullable, sourceLine, sourceColumn)
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

        throw new VariableTypeException(value.TypeName, VariableTypeName, sourceLine, sourceColumn);
    }
}
