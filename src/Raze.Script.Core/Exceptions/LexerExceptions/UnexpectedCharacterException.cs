using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.LexerExceptions;

public sealed class UnexpectedCharacterException
    : LexerException, IThrowableByThrowHelper<UnexpectedCharacterException>
{
    private UnexpectedCharacterException(string message, SourceInfo source)
        : base(message, source, nameof(UnexpectedCharacterException))
    {
    }

    static UnexpectedCharacterException IThrowableByThrowHelper<UnexpectedCharacterException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new UnexpectedCharacterException(message, source);
    }
}
