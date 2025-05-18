namespace Raze.Script.Core.Exceptions;

public abstract class RuntimeException : RazeException
{
    public RuntimeException(string error, int line, int column, string className)
        : base(error, line, column, className)
    {
    }
}
