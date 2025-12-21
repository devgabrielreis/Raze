using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Symbols;

namespace Raze.Script.Core.Runtime.Scopes;

public abstract class Scope
{
    protected Scope? _parent;

    protected Dictionary<string, VariableSymbol> _variables;

    protected abstract bool CanDeclareVariables { get; }

    protected abstract bool CanDeclareConstants { get; }

    public Scope(Scope? parent)
    {
        _parent = parent;
        _variables = [];
    }

    public virtual void DeclareVariable(string name, VariableSymbol variable, SourceInfo source)
    {
        if (!CanDeclareVariables && !variable.IsConstant)
        {
            throw new ScopeDeclarationException("variable", this.GetType().Name, source);
        }

        if (!CanDeclareConstants && variable.IsConstant)
        {
            throw new ScopeDeclarationException("constant", this.GetType().Name, source);
        }

        if (_variables.ContainsKey(name))
        {
            throw new RedeclarationException($"Symbol {name} is already declared", source);
        }

        _variables[name] = variable;
    }

    public virtual VariableSymbol GetVariable(string name, SourceInfo source, bool throwIfNotInitialized = true)
    {
        var variable = TryGetVariable(name);

        if (variable is null)
        {
            throw new UndefinedIdentifierException(name, source);
        }

        if (throwIfNotInitialized && !variable.IsInitialized)
        {
            throw new UninitializedVariableException(source);
        }

        return variable;
    }

    public virtual VariableSymbol? TryGetVariable(string name)
    {
        if (_variables.TryGetValue(name, out var value))
        {
            return value;
        }

        if (_parent is not null)
        {
            return _parent.TryGetVariable(name);
        }

        return null;
    }
}
