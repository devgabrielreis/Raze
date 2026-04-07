using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Scopes;
using Raze.Script.Core.Runtime.Symbols;

namespace Raze.Script.Core.Builders;

internal sealed class ModuleBuilder
{
    private string? _moduleName;
    private Dictionary<string, VariableSymbol> _members;

    internal ModuleBuilder()
    {
        _moduleName = null;
        _members = [];
    }

    internal ModuleBuilder HasName(string moduleName)
    {
        if (_moduleName != null)
        {
            throw new InvalidOperationException(
                $"Module name is already set to \"{_moduleName}\". It cannot be changed to \"{moduleName}\""
            );
        }

        _moduleName = moduleName;

        return this;
    }

    internal ModuleBuilder HasConstant(Action<ConstantBuilder> builderFunction)
    {
        var constantBuilder = new ConstantBuilder();

        builderFunction(constantBuilder);

        var constant = constantBuilder.Build();

        if (!_members.TryAdd(constantBuilder.ConstantName, constant))
        {
            throw new InvalidOperationException(
                $"A member with the name \"{constantBuilder.ConstantName}\" has already been defined in this module"
            );
        }

        return this;
    }

    internal NamespaceSymbol Build()
    {
        if (_moduleName is null)
        {
            throw new InvalidOperationException($"Module name must be set by calling \"{nameof(HasName)}()\"");
        }

        var source = new SourceInfo($"{_moduleName} builder");
        var moduleScope = Scope.CreateNamespaceScope(null);

        foreach (var item in _members)
        {
            moduleScope.DeclareVariable(item.Key, item.Value, in source);
        }

        return new NamespaceSymbol(moduleScope);
    }
}
