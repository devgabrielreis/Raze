using Raze.Script.Core.Builders;
using Raze.Script.Core.BuiltInModules;
using Raze.Script.Core.Engine;
using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Scopes;
using Raze.Script.Core.Runtime.Symbols;

namespace Raze.Script.Core;

public sealed class RazeSession
{
    public string ScopeKind => RootScope.Kind;

    internal readonly Scope RootScope;

    private readonly Dictionary<string, Action<ModuleBuilder>>? _customModuleBuilders;
    private readonly HashSet<string> _readFiles;
    private readonly HashSet<string> _importedModules;

    public RazeSession(Dictionary<string, Action<ModuleBuilder>>? customModuleBuilders = null)
        : this(Scope.CreateInterpreterScope(), customModuleBuilders)
    {
    }

    internal RazeSession(Scope mainScope, Dictionary<string, Action<ModuleBuilder>>? customModuleBuilders = null)
    {
        if (customModuleBuilders != null)
        {
            ValidateCustomModuleBuilders(customModuleBuilders);
        }

        RootScope = mainScope;
        _customModuleBuilders = customModuleBuilders;
        _readFiles = new HashSet<string>();
        _importedModules = new HashSet<string>();
    }

    internal NamespaceSymbol? GetCustomModule(string name)
    {
        if (
            _customModuleBuilders != null
            && _customModuleBuilders.TryGetValue(name, out var moduleBuilderFunction)
        )
        {
            var moduleBuilder = new ModuleBuilder(name);
            moduleBuilderFunction(moduleBuilder);

            return moduleBuilder.Build();
        }

        return null;
    }

    internal void RegisterReadFile(string fileFullPath)
    {
        _readFiles.Add(fileFullPath);
    }

    internal bool HasReadFile(string fileFullPath)
    {
        return _readFiles.Contains(fileFullPath);
    }

    internal void RegisterImportedModule(string moduleName)
    {
        _importedModules.Add(moduleName);
    }

    internal bool HasImportedModule(string moduleName)
    {
        return _importedModules.Contains(moduleName);
    }

    private static void ValidateCustomModuleBuilders(Dictionary<string, Action<ModuleBuilder>> customModuleBuilders)
    {
        foreach (var customModuleName in customModuleBuilders.Keys)
        {
            if (BuiltInModuleManager.HasModule(customModuleName))
            {
                var source = new SourceInfo($"{nameof(Interpreter)} initializer");
                ThrowHelper.Throw<RedeclarationException>(
                    $"The custom module \"{customModuleName}\" has the same name as a built in module",
                    in source
                );
            }
        }
    }
}
