using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class UnsupportedUnaryOperationException
    : RuntimeException, IThrowableByThrowHelper<UnsupportedUnaryOperationException>
{
    private UnsupportedUnaryOperationException(string message, SourceInfo source)
        : base(
            message,
            source,
            nameof(UnsupportedUnaryOperationException)
        )
    {
    }

    static UnsupportedUnaryOperationException IThrowableByThrowHelper<UnsupportedUnaryOperationException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new UnsupportedUnaryOperationException(message, source);
    }
}
