using Raze.Script.Core.Types;

namespace Raze.Script.Core.Symbols.Variables;

public abstract class VariableSymbol : Symbol
{
    public RuntimeType Value { get; protected set; }

    public abstract void SetNewValue(RuntimeType value);
}
