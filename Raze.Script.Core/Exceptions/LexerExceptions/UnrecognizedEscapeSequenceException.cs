namespace Raze.Script.Core.Exceptions.LexerExceptions;

public class UnrecognizedEscapeSequenceException : LexerException
{
    public UnrecognizedEscapeSequenceException(string escapeSequence, int line, int column)
        : base(escapeSequence, line, column, nameof(UnrecognizedEscapeSequenceException))
    {
    }
}
