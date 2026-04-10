using Raze.Script.Core.Builders;
using Raze.Script.Core.Runtime.Symbols;

namespace Raze.Script.Core.BuiltInModules;

internal static class BuiltInModuleManager
{
    private static readonly Dictionary<string, NamespaceSymbol> _loadedModules = [];
    private static readonly Dictionary<string, Action<ModuleBuilder>> _moduleBuilderFunctions = new()
    {
        [MathModule.Name] = MathModule.Build
    };

    internal static NamespaceSymbol? TryGetModule(string name)
    {
        lock (_loadedModules)
        {
            if (_loadedModules.TryGetValue(name, out var loadedModule))
            {
                return loadedModule;
            }

            if (_moduleBuilderFunctions.TryGetValue(name, out var moduleBuilderFunction))
            {
                var moduleBuilder = new ModuleBuilder(name);
                moduleBuilderFunction(moduleBuilder);

                var builtModule = moduleBuilder.Build();
                _loadedModules.Add(name, builtModule);

                return builtModule;
            }

            return null;
        }
    }

    internal static bool HasModule(string name)
    {
        return _moduleBuilderFunctions.ContainsKey(name);
    }
}
