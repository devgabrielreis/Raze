using Raze.Script.Core.Runtime.Scopes;

namespace Raze.Script.Core.Runtime.Symbols;

internal class NamespaceSymbol : Symbol
{
    internal NamespaceScope Scope { get; private set; }

    internal NamespaceSymbol(NamespaceScope namespaceScope)
    {
        Scope = namespaceScope;
    }
}
