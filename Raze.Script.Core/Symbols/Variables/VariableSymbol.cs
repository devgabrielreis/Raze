using Raze.Script.Core.Types;

namespace Raze.Script.Core.Symbols.Variables;

public abstract class VariableSymbol : Symbol
{
    public RuntimeType Value { get; protected set; }

    public bool IsConstant { get; private set; }

    public VariableSymbol(RuntimeType value, bool isConstant)
    {
        SetValue(value);
        IsConstant = isConstant;
    }

    public virtual void SetNewValue(RuntimeType newValue)
    {
        if (IsConstant)
        {
            throw new Exception("tentando mudar o valor de uma constante");
        }

        SetValue(newValue);
    }

    protected abstract void SetValue(RuntimeType value);
}
