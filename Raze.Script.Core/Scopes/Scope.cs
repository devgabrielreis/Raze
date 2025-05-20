using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements;
using Raze.Script.Core.Symbols;
using Raze.Script.Core.Symbols.Variables;
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

    internal virtual void DeclareVariable(string name, VariableSymbol variable, VariableDeclarationStatement source)
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

        if (Lookup(name) is not null)
        {
            throw new RedeclarationException($"Symbol {name} is already declared", sourceLine, sourceColumn);
        }

        _variables[name] = variable;
    }

    internal virtual void AssignVariable(string name, RuntimeValue value, Statement source)
    {
        AssignVariable(name, value, source.StartLine, source.StartLine);
    }

    public virtual void AssignVariable(string name, RuntimeValue value)
    {
        AssignVariable(name, value, null, null);
    }

    protected virtual void AssignVariable(string name, RuntimeValue value, int? line, int? column)
    {
        var resolvedScope = Resolve(name);

        if (resolvedScope is null)
        {
            throw new UndefinedIdentifierException(name, line, column);
        }

        resolvedScope._variables[name].SetNewValue(value);
    }

    public virtual Symbol? Lookup(string symbol)
    {
        _variables.TryGetValue(symbol, out var result);

        return result;
    }

    public virtual Scope? Resolve(string symbol)
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
