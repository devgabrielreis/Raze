namespace Raze.Script.Core.Exceptions.LexerExceptions;

public class InvalidStringException : LexerException
{
    public InvalidStringException(string stringToken, int line, int column)
        : base($"Invalid string declaration: {stringToken}", line, column, nameof(InvalidStringException))
    {
    }
}
