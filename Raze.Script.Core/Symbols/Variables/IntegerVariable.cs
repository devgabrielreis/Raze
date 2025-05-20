using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Types;

namespace Raze.Script.Core.Symbols.Variables;

public class IntegerVariable : VariableSymbol
{
    public IntegerVariable(RuntimeType value, bool isConstant)
        : base(value, isConstant)
    {
    }

    protected override void SetValue(RuntimeType value)
    {
        if (value is NullType)
        {
            Value = new IntegerType(null);
            return;
        }
        else if (value is IntegerType)
        {
            Value = value;
            return;
        }

        throw new VariableTypeException(value.TypeName, nameof(IntegerVariable), 0, 0);
    }
}
