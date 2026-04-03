using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public sealed class InvalidTokenListException
    : ParseException, IThrowableByThrowHelper<InvalidTokenListException>
{
    private InvalidTokenListException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidTokenListException))
    {
    }

    static InvalidTokenListException IThrowableByThrowHelper<InvalidTokenListException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new InvalidTokenListException(message, source);
    }
}
