using Raze.Script.Core.Runtime.Scopes;

namespace Raze.Script.Core.Runtime.Symbols;

internal sealed class NamespaceSymbol
{
    internal Scope Scope { get; }

    internal NamespaceSymbol(Scope namespaceScope)
    {
        Scope = namespaceScope;
    }
}
