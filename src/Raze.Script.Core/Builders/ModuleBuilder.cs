using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Scopes;
using Raze.Script.Core.Runtime.Symbols;

namespace Raze.Script.Core.Builders;

internal sealed class ModuleBuilder
{
    private string _moduleName;
    private Dictionary<string, VariableSymbol> _members;

    internal ModuleBuilder(string moduleName)
    {
        _moduleName = moduleName;
        _members = [];
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
        var source = new SourceInfo($"{_moduleName} builder");
        var moduleScope = Scope.CreateNamespaceScope(null);

        foreach (var item in _members)
        {
            moduleScope.DeclareVariable(item.Key, item.Value, in source);
        }

        return new NamespaceSymbol(moduleScope);
    }
}
