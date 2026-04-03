using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class UndefinedIdentifierException : RuntimeException
{
    internal UndefinedIdentifierException(string message, SourceInfo source)
        : base(message, source, nameof(UndefinedIdentifierException))
    {
    }
}
