using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class UnexpectedTypeException
    : RuntimeException, IThrowableByThrowHelper<UnexpectedTypeException>
{
    private UnexpectedTypeException(string message, SourceInfo source)
        : base(message, source, nameof(UnexpectedTypeException))
    {
    }

    static UnexpectedTypeException IThrowableByThrowHelper<UnexpectedTypeException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new UnexpectedTypeException(message, source);
    }
}
