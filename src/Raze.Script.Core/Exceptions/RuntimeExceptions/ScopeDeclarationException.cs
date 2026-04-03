using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class ScopeDeclarationException : RuntimeException
{
    internal ScopeDeclarationException(string message, SourceInfo source)
        : base(message, source, nameof(ScopeDeclarationException))
    {
    }
}
