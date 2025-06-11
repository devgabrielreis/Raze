namespace Raze.Script.Core.Exceptions.InterpreterExceptions;

public abstract class InterpreterException : RazeException
{
    public InterpreterException(string error, int line, int column, string className)
        : base(error, line, column, className)
    {
    }
}
