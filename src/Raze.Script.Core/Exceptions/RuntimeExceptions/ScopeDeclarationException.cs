using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class ScopeDeclarationException : RuntimeException
{
    internal ScopeDeclarationException(string structureName, string scopeType, SourceInfo source)
        : base($"Cannot declare {structureName} on {scopeType}", source, nameof(ScopeDeclarationException))
    {
    }
}
