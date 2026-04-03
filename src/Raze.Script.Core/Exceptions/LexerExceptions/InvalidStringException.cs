using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.LexerExceptions;

public sealed class InvalidStringException
    : LexerException, IThrowableByThrowHelper<InvalidStringException>
{
    private InvalidStringException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidStringException))
    {
    }

    static InvalidStringException IThrowableByThrowHelper<InvalidStringException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new InvalidStringException(message, source);
    }
}
