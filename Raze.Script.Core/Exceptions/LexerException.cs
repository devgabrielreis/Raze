namespace Raze.Script.Core.Exceptions;

public abstract class LexerException : RazeException
{
    public LexerException(string error, int line, int column, string className)
        : base(error, line, column, className)
    {
    }
}
