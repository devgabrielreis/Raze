using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class UnsupportedBinaryOperationException
    : RuntimeException, IThrowableByThrowHelper<UnsupportedBinaryOperationException>
{
    private UnsupportedBinaryOperationException(string message, SourceInfo source)
        : base(
            message,
            source,
            nameof(UnsupportedBinaryOperationException)
        )
    {
    }

    static UnsupportedBinaryOperationException IThrowableByThrowHelper<UnsupportedBinaryOperationException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new UnsupportedBinaryOperationException(message, source);
    }
}
