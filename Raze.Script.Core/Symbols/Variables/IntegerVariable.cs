using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Symbols.Variables;

public class IntegerVariable : VariableSymbol
{
    public override IntegerValue? Value => _value;

    private IntegerValue? _value = null;

    public override string VariableTypeName => IsNullable ? "NullableIntegerVariable" : "IntegerVariable";

    public IntegerVariable(RuntimeValue? value, bool isConstant, bool isNullable)
        : base(value, isConstant, isNullable)
    {
    }

    internal IntegerVariable(RuntimeValue? value, bool isConstant, bool isNullable, int? sourceLine, int? sourceColumn)
        : base(value, isConstant, isNullable, sourceLine, sourceColumn)
    {
    }

    protected override void SetValue(RuntimeValue value, int? sourceLine, int? sourceColumn)
    {
        if (value is NullValue)
        {
            _value = new IntegerValue(null);
            return;
        }
        else if (value is IntegerValue intValue)
        {
            _value = intValue;
            return;
        }

        throw new VariableTypeException(value.TypeName, VariableTypeName, sourceLine, sourceColumn);
    }
}
