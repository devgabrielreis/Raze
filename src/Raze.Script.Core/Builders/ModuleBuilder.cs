using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Scopes;
using Raze.Script.Core.Runtime.Symbols;

namespace Raze.Script.Core.Builders;

public sealed class ModuleBuilder
{
    private string _moduleName;
    private Dictionary<string, VariableSymbol> _members;

    internal ModuleBuilder(string moduleName)
    {
        if (string.IsNullOrEmpty(moduleName))
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"Module name cannot be null or empty"
            );
        }

        _moduleName = moduleName;
        _members = [];
    }

    public ModuleBuilder HasConstant(Action<ConstantBuilder> builderFunction)
    {
        var constantBuilder = new ConstantBuilder();

        builderFunction(constantBuilder);

        var constant = constantBuilder.Build();

        if (!_members.TryAdd(constantBuilder.ConstantName, constant))
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"A member with the name \"{constantBuilder.ConstantName}\" has already been defined in this module"
            );
        }

        return this;
    }

    public ModuleBuilder HasFunction(Action<SystemFunctionBuilder> builderFunction)
    {
        var functionBuilder = new SystemFunctionBuilder();

        builderFunction(functionBuilder);

        var function = functionBuilder.Build();

        if (!_members.TryAdd(functionBuilder.FunctionName, function))
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"A member with the name \"{functionBuilder.FunctionName}\" has already been defined in this module"
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
