using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Symbols;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Raze.Script.Core.Runtime.Scopes;

internal sealed class Scope
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

    internal readonly string Kind;

    private readonly Scope? _parent;

    private readonly Dictionary<string, VariableSymbol> _variables;

    private readonly Dictionary<string, NamespaceSymbol> _namespaces;

    private readonly ScopePermissions _permissions;

    private Scope(Scope? parent, ScopePermissions permissions, string kind)
    {
        _parent = parent;
        _variables = [];
        _namespaces = [];
        _permissions = permissions;
        Kind = kind;
    }

    internal static Scope CreateInterpreterScope()
    {
        return new Scope(null, ScopePermissions.All, "Interpreter");
    }

    internal static Scope CreateGlobalScope()
    {
        return new Scope(
            null,
            ScopePermissions.DeclareConstants | ScopePermissions.DeclareNamespaces,
            "Global"
        );
    }

    internal static Scope CreateLocalScope(Scope parent)
    {
        return new Scope(
            parent,
            ScopePermissions.DeclareConstants | ScopePermissions.DeclareVariables,
            "Local"
        );
    }

    internal static Scope CreateNamespaceScope(Scope? parent)
    {
        return new Scope(parent, ScopePermissions.DeclareConstants, "Namespace");
    }

    internal void ThrowIfImportIsNotAllowed(ref readonly SourceInfo source)
    {
        if (!Can(ScopePermissions.DeclareNamespaces))
        {
            ThrowHelper.Throw<ScopeDeclarationException>(
                $"Cannot use imports on {Kind.ToLower()} scope",
                in source
            );
        }
    }

    internal void DeclareVariable(string name, VariableSymbol variable, ref readonly SourceInfo source)
    {
        var permissionNeeded = variable.IsConstant
                                ? ScopePermissions.DeclareConstants
                                : ScopePermissions.DeclareVariables;

        if (!Can(permissionNeeded))
        {
            var variableKind = variable.IsConstant ? "constant" : "variable";
            ThrowHelper.Throw<ScopeDeclarationException>(
                $"Cannot declare {variableKind} on {Kind.ToLower()} scope",
                in source
            );
        }

        if (!_variables.TryAdd(name, variable))
        {
            ThrowHelper.Throw<RedeclarationException>(
                $"The variable {name} is already declared",
                in source
            );
        }
    }

    internal VariableSymbol GetVariable(string name, ref readonly SourceInfo source, bool throwIfNotInitialized = false)
    {
        var variable = TryGetVariable(name);

        if (variable is null)
        {
            HandleUndefinedSymbol(name, "a value", in source);
        }

        if (throwIfNotInitialized && !variable.IsInitialized)
        {
            ThrowHelper.Throw<UninitializedVariableException>(
                "Trying to access variable before its initialization",
                in source
            );
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

    internal void DeclareNamespace(
        string name,
        NamespaceSymbol namespaceSymbol,
        ref readonly SourceInfo source,
        bool throwIfAlreadyDeclared = true
    )
    {
        if (!Can(ScopePermissions.DeclareNamespaces))
        {
            ThrowHelper.Throw<ScopeDeclarationException>(
                $"Cannot declare namespace on {Kind.ToLower()} scope",
                in source
            );
        }

        if (!_namespaces.TryAdd(name, namespaceSymbol))
        {
            if (!throwIfAlreadyDeclared)
            {
                return;
            }

            ThrowHelper.Throw<RedeclarationException>(
                $"The namespace {name} is already declared",
                in source
            );
        }
    }

    internal NamespaceSymbol GetNamespace(string name, ref readonly SourceInfo source)
    {
        var namespaceSymbol = TryGetNamespace(name);

        if (namespaceSymbol is null)
        {
            HandleUndefinedSymbol(name, "a namespace", in source);
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
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void HandleUndefinedSymbol(string name, string wasBeingUsedAs, ref readonly SourceInfo source)
    {
        if (TryGetNamespace(name) != null)
        {
            ThrowHelper.Throw<UnexpectedStatementException>(
                $"{name} is a namespace, but it is being used as {wasBeingUsedAs}",
                in source
            );
        }

        if (TryGetVariable(name) is VariableSymbol variableSymbol)
        {
            var varType = variableSymbol.IsConstant ? "constant" : "variable";
            ThrowHelper.Throw<UnexpectedStatementException>(
                $"{name} is a {varType}, but it is being used as {wasBeingUsedAs}",
                in source
            );
        }

        ThrowHelper.Throw<UndefinedIdentifierException>(
            $"Undefined identifier: {name}",
            in source
        );
    }
}
