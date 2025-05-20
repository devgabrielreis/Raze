namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public abstract class RuntimeException : RazeException
{
    public RuntimeException(string error, int? line, int? column, string className)
        : base(error, line, column, className)
    {
    }

    public RuntimeException(int? line, int? column, string className)
        : base(line, column, className)
    {
    }
}
