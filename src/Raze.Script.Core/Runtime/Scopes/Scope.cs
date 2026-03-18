using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Symbols;

namespace Raze.Script.Core.Runtime.Scopes;

public abstract class Scope
{
    private Scope? _parent;

    private Dictionary<string, VariableSymbol> _variables;

    private Dictionary<string, NamespaceSymbol> _namespaces;

    protected abstract bool CanDeclareVariables { get; }

    protected abstract bool CanDeclareConstants { get; }

    protected abstract bool CanDeclareNamespaces { get; }

    public Scope(Scope? parent)
    {
        _parent = parent;
        _variables = [];
        _namespaces = [];
    }

    internal virtual void DeclareVariable(string name, VariableSymbol variable, SourceInfo source)
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
            throw new RedeclarationException($"The variable {name} is already declared", source);
        }

        _variables[name] = variable;
    }

    internal virtual VariableSymbol GetVariable(string name, SourceInfo source, bool throwIfNotInitialized = false)
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

    internal virtual VariableSymbol? TryGetVariable(string name)
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

    internal virtual void DeclareNamespace(string name, NamespaceSymbol namespaceSymbol, SourceInfo source)
    {
        if (!CanDeclareNamespaces)
        {
            throw new ScopeDeclarationException("namespace", this.GetType().Name, source);
        }

        if (_namespaces.ContainsKey(name))
        {
            throw new RedeclarationException($"The namespace {name} is already declared", source);
        }

        _namespaces[name] = namespaceSymbol;
    }

    internal virtual NamespaceSymbol GetNamespace(string name, SourceInfo source)
    {
        var namespaceSymbol = TryGetNamespace(name);

        if (namespaceSymbol is null)
        {
            throw new UndefinedIdentifierException(name, source);
        }

        return namespaceSymbol;
    }

    internal virtual NamespaceSymbol? TryGetNamespace(string name)
    {
        if (_namespaces.TryGetValue(name, out var namespaceSymbol))
        {
            return namespaceSymbol;
        }

        if (_parent is not null)
        {
            return _parent.TryGetNamespace(name);
        }

        return null;
    }
}
