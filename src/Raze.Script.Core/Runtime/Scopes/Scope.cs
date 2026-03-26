using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Symbols;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Raze.Script.Core.Runtime.Scopes;

public sealed class Scope
{
    [Flags]
    private enum ScopePermissions : byte
    {
        None = 0,
        DeclareVariables = 1 << 0,
        DeclareConstants = 1 << 1,
        DeclareNamespaces = 1 << 2,
        All = DeclareVariables | DeclareConstants | DeclareNamespaces
    }

    private Scope? _parent;

    private Dictionary<string, VariableSymbol> _variables;

    private Dictionary<string, NamespaceSymbol> _namespaces;

    private ScopePermissions _permissions;

    private string _kind;

    private Scope(Scope? parent, ScopePermissions permissions, string kind)
    {
        _parent = parent;
        _variables = [];
        _namespaces = [];
        _permissions = permissions;
        _kind = kind;
    }

    internal static Scope CreateInterpreterScope()
    {
        return new Scope(null, ScopePermissions.All, "InterpreterScope");
    }

    internal static Scope CreateLocalScope(Scope parent)
    {
        return new Scope(
            parent,
            ScopePermissions.DeclareConstants | ScopePermissions.DeclareVariables,
            "LocalScope"
        );
    }

    internal static Scope CreateNamespaceScope(Scope parent)
    {
        return new Scope(parent, ScopePermissions.DeclareConstants, "NamespaceScope");
    }

    internal void DeclareVariable(string name, VariableSymbol variable, SourceInfo source)
    {
        var permissionNeeded = variable.IsConstant
                                ? ScopePermissions.DeclareConstants
                                : ScopePermissions.DeclareVariables;

        if (!Can(permissionNeeded))
        {
            var variableKind = variable.IsConstant ? "constant" : "variable";

            ThrowScopeDeclarationException(variableKind, in source);
        }

        if (!_variables.TryAdd(name, variable))
        {
            ThrowRedeclarationException($"The variable {name} is already declared", in source);
        }
    }

    internal VariableSymbol GetVariable(string name, SourceInfo source, bool throwIfNotInitialized = false)
    {
        var variable = TryGetVariable(name);

        if (variable is null)
        {
            ThrowUndefinedIdentifierException(name, in source);
        }

        if (throwIfNotInitialized && !variable.IsInitialized)
        {
            ThrowUninitializedVariableException(in source);
        }

        return variable;
    }

    internal VariableSymbol? TryGetVariable(string name)
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

    internal void DeclareNamespace(string name, NamespaceSymbol namespaceSymbol, SourceInfo source)
    {
        if (!Can(ScopePermissions.DeclareNamespaces))
        {
            ThrowScopeDeclarationException("namespace", in source);
        }

        if (!_namespaces.TryAdd(name, namespaceSymbol))
        {
            ThrowRedeclarationException($"The namespace {name} is already declared", in source);
        }
    }

    internal NamespaceSymbol GetNamespace(string name, SourceInfo source)
    {
        var namespaceSymbol = TryGetNamespace(name);

        if (namespaceSymbol is null)
        {
            ThrowUndefinedIdentifierException(name, in source);
        }

        return namespaceSymbol;
    }

    internal NamespaceSymbol? TryGetNamespace(string name)
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

    private bool Can(ScopePermissions permission)
    {
        return _permissions.HasFlag(permission);
    }

    [DoesNotReturn]
    [StackTraceHidden]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void ThrowScopeDeclarationException(string invalidSymbolType, ref readonly SourceInfo source)
    {
        throw new ScopeDeclarationException(invalidSymbolType, _kind, source);
    }

    [DoesNotReturn]
    [StackTraceHidden]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowRedeclarationException(string message, ref readonly SourceInfo source)
    {
        throw new RedeclarationException(message, source);
    }

    [DoesNotReturn]
    [StackTraceHidden]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowUndefinedIdentifierException(string name, ref readonly SourceInfo source)
    {
        throw new UndefinedIdentifierException(name, source);
    }

    [DoesNotReturn]
    [StackTraceHidden]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowUninitializedVariableException(ref readonly SourceInfo source)
    {
        throw new UninitializedVariableException(source);
    }
}
