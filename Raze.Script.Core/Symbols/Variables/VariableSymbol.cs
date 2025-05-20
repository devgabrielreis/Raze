using Raze.Script.Core.Values;

namespace Raze.Script.Core.Symbols.Variables;

public abstract class VariableSymbol : Symbol
{
    public RuntimeValue Value { get; protected set; }

    public bool IsConstant { get; private set; }

    public VariableSymbol(RuntimeValue value, bool isConstant)
    {
        SetValue(value);
        IsConstant = isConstant;
    }

    public virtual void SetNewValue(RuntimeValue newValue)
    {
        if (IsConstant)
        {
            throw new Exception("tentando mudar o valor de uma constante");
        }

        SetValue(newValue);
    }

    protected abstract void SetValue(RuntimeValue value);
}
