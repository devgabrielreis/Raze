using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public sealed class UnexpectedTokenException
    : ParseException, IThrowableByThrowHelper<UnexpectedTokenException>
{
    private UnexpectedTokenException(string message, SourceInfo source)
        : base(message, source, nameof(UnexpectedTokenException))
    {
    }

    static UnexpectedTokenException IThrowableByThrowHelper<UnexpectedTokenException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new UnexpectedTokenException(message, source);
    }
}
