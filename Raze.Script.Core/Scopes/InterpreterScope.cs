using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Symbols.Variables;
using Raze.Script.Core.Types;

namespace Raze.Script.Core.Scopes;

public class InterpreterScope : Scope
{
    public InterpreterScope()
        : base(null)
    {
    }

    public override void DeclareVariable(string name, VariableSymbol variable)
    {
        if (Lookup(name) is not null)
        {
            throw new RedeclarationException($"Symbol {name} is already declared", 0, 0);
        }

        _variables[name] = variable;
    }

    public override void AssignVariable(string name, RuntimeType value)
    {
        if (Lookup(name) is null)
        {
            throw new UndefinedIdentifierException(name, 0, 0);
        }

        _variables[name].SetNewValue(value);
    }
}
