namespace Raze.Script.Core.Exceptions.LexerExceptions;

public abstract class LexerException : RazeException
{
    public LexerException(string error, int line, int column, string className)
        : base(error, line, column, className)
    {
    }
}
