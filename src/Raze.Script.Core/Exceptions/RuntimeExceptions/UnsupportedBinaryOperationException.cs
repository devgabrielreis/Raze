using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class UnsupportedBinaryOperationException : RuntimeException
{
    internal UnsupportedBinaryOperationException(string message, SourceInfo source)
        : base(
            message,
            source,
            nameof(UnsupportedBinaryOperationException)
        )
    {
    }
}
