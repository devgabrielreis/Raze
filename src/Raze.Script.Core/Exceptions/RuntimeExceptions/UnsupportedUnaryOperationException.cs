using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class UnsupportedUnaryOperationException : RuntimeException
{
    internal UnsupportedUnaryOperationException(string message, SourceInfo source)
        : base(
            message,
            source,
            nameof(UnsupportedUnaryOperationException)
        )
    {
    }
}
