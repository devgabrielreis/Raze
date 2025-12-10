using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class RedeclarationException : RuntimeException
{
    internal RedeclarationException(string error, SourceInfo source)
        : base(error, source, nameof(RedeclarationException))
    {
    }
}
