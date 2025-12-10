using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.LexerExceptions;

public class UnexpectedCharacterException : LexerException
{
    internal UnexpectedCharacterException(char c, SourceInfo source)
        : base($"Unexpected character found: {c}", source, nameof(UnexpectedCharacterException))
    {
    }
}
