using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements;
using Raze.Script.Core.Symbols;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Scopes;

public abstract class Scope
{
    protected Scope? _parent;

    protected Dictionary<string, VariableSymbol> _variables;

    protected abstract bool CanDeclareVariables { get; }

    protected abstract bool CanDeclareConstants { get; }

    public Scope(Scope? parent)
    {
        _parent = parent;
        _variables = new();
    }

    public virtual void DeclareVariable(string name, VariableSymbol variable)
    {
        DeclareVariable(name, variable, null, null);
    }

    internal virtual void DeclareVariable(string name, VariableSymbol variable, Statement source)
    {
        DeclareVariable(name, variable, source.StartLine, source.StartColumn);
    }

    protected virtual void DeclareVariable(string name, VariableSymbol variable, int? sourceLine, int? sourceColumn)
    {
        if (!CanDeclareVariables && !variable.IsConstant)
        {
            throw new ScopeDeclarationException("variable", this.GetType().Name, sourceLine, sourceColumn);
        }

        if (!CanDeclareConstants && variable.IsConstant)
        {
            throw new ScopeDeclarationException("constant", this.GetType().Name, sourceLine, sourceColumn);
        }

        if (FindSymbol(name) is not null)
        {
            throw new RedeclarationException($"Symbol {name} is already declared", sourceLine, sourceColumn);
        }

        _variables[name] = variable;
    }

    internal virtual void AssignVariable(string name, RuntimeValue value, Statement source)
    {
        AssignVariable(name, value, source.StartLine, source.StartColumn);
    }

    public virtual void AssignVariable(string name, RuntimeValue value)
    {
        AssignVariable(name, value, null, null);
    }

    protected virtual void AssignVariable(string name, RuntimeValue value, int? line, int? column)
    {
        var resolvedScope = FindSymbolScope(name);

        if (resolvedScope is null)
        {
            throw new UndefinedIdentifierException(name, line, column);
        }

        resolvedScope._variables[name].SetValue(value, line, column);
    }

    public virtual Symbol? FindSymbol(string symbol)
    {
        if (_variables.TryGetValue(symbol, out var result))
        {
            return result;
        }

        return null;
    }

    public virtual Scope? FindSymbolScope(string symbol)
    {
        if (FindSymbol(symbol) is not null)
        {
            return this;
        }

        if (_parent is not null && _parent.FindSymbolScope(symbol) is Scope result)
        {
            return result;
        }

        return null;
    }
}
