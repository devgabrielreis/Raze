using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class RedeclarationException : RuntimeException
{
    internal RedeclarationException(string message, SourceInfo source)
        : base(message, source, nameof(RedeclarationException))
    {
    }
}
