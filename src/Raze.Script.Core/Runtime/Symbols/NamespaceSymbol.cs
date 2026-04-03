using Raze.Script.Core.Runtime.Scopes;

namespace Raze.Script.Core.Runtime.Symbols;

internal sealed class NamespaceSymbol
{
    internal readonly Scope Scope;

    internal NamespaceSymbol(Scope namespaceScope)
    {
        Scope = namespaceScope;
    }
}
