using Raze.Script.Core.Symbols;
using Raze.Script.Core.Symbols.Variables;
using Raze.Script.Core.Types;

namespace Raze.Script.Core.Scopes;

public abstract class Scope
{
    protected Scope? _parent;

    protected Dictionary<string, VariableSymbol> _variables;

    public Scope(Scope? parent)
    {
        _parent = parent;
        _variables = new();
    }

    public abstract void DeclareVariable(string name, VariableSymbol variable);

    public abstract void AssignVariable(string name, RuntimeType value);

    public Symbol? Lookup(string symbol)
    {
        _variables.TryGetValue(symbol, out var result);

        return result;
    }

    public Scope? Resolve(string symbol)
    {
        if (Lookup(symbol) is not null)
        {
            return this;
        }

        if (_parent is not null && _parent.Lookup(symbol) is not null)
        {
            return _parent;
        }

        return null;
    }
}
