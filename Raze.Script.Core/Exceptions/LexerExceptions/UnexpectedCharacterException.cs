namespace Raze.Script.Core.Exceptions.LexerExceptions;

public class UnexpectedCharacterException : LexerException
{
    public UnexpectedCharacterException(char c, int line, int column)
        : base($"Unexpected character found: {c}", line, column, nameof(UnexpectedCharacterException))
    {
    }
}
