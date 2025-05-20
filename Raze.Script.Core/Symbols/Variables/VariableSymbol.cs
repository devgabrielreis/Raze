using Raze.Script.Core.Exceptions.RuntimeExceptions;
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
        SetNewValue(newValue, null, null);
    }

    internal virtual void SetNewValue(RuntimeValue newValue, int? sourceLine, int? sourceColumn)
    {
        if (IsConstant)
        {
            throw new ConstantAssignmentException(sourceLine, sourceColumn);
        }

        SetValue(newValue);
    }

    protected abstract void SetValue(RuntimeValue value);
}
