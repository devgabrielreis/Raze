using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Symbols.Variables;

public class StringVariable : VariableSymbol
{
    public override StringValue? Value => _value;

    private StringValue? _value = null;

    public override string VariableTypeName => IsNullable ? "NullableStringVariable" : "StringVariable";

    public StringVariable(RuntimeValue? value, bool isConstant, bool isNullable)
        : base(value, isConstant, isNullable)
    {
    }

    internal StringVariable(RuntimeValue? value, bool isConstant, bool isNullable, int? sourceLine, int? sourceColumn)
        : base(value, isConstant, isNullable, sourceLine, sourceColumn)
    {
    }

    protected override void SetValue(RuntimeValue value, int? sourceLine, int? sourceColumn)
    {
        if (value is NullValue)
        {
            _value = new StringValue(null);
            return;
        }
        else if (value is StringValue strValue)
        {
            _value = strValue;
            return;
        }

        throw new VariableTypeException(value.TypeName, VariableTypeName, sourceLine, sourceColumn);
    }
}
