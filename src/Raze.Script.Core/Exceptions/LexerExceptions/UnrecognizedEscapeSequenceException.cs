using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.LexerExceptions;

public sealed class UnrecognizedEscapeSequenceException
    : LexerException, IThrowableByThrowHelper<UnrecognizedEscapeSequenceException>
{
    private UnrecognizedEscapeSequenceException(string message, SourceInfo source)
        : base(message, source, nameof(UnrecognizedEscapeSequenceException))
    {
    }

    static UnrecognizedEscapeSequenceException IThrowableByThrowHelper<UnrecognizedEscapeSequenceException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new UnrecognizedEscapeSequenceException(message, source);
    }
}
