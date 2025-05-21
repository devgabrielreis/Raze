using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Symbols.Variables;

public abstract class VariableSymbol : Symbol
{
    public RuntimeValue Value { get; protected set; }

    public bool IsConstant { get; private set; }

    public VariableSymbol(RuntimeValue value, bool isConstant)
    {
        IsConstant = false;
        SetNewValue(value, null, null);
        IsConstant = isConstant;
    }

    internal VariableSymbol(RuntimeValue value, bool isConstant, int? sourceLine, int? sourceColumn)
    {
        IsConstant = false;
        SetNewValue(value, sourceLine, sourceColumn);
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

        SetValue(newValue, sourceLine, sourceColumn);
    }

    protected abstract void SetValue(RuntimeValue value, int? sourceLine, int? sourceColumn);
}
