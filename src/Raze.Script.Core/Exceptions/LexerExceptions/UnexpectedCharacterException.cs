using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.LexerExceptions;

public sealed class UnexpectedCharacterException : LexerException
{
    internal UnexpectedCharacterException(string message, SourceInfo source)
        : base(message, source, nameof(UnexpectedCharacterException))
    {
    }
}
