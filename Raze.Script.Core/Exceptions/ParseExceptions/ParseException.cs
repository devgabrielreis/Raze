namespace Raze.Script.Core.Exceptions.ParseExceptions;

public abstract class ParseException : RazeException
{
    public ParseException(string error, int line, int column, string className)
        : base(error, line, column, className)
    {
    }
}
