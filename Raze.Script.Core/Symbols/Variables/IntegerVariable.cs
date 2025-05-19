using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Types;

namespace Raze.Script.Core.Symbols.Variables;

public class IntegerVariable : VariableSymbol
{
    public IntegerVariable(RuntimeType value)
    {
        SetNewValue(value);
    }

    public override void SetNewValue(RuntimeType value)
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
