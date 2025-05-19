namespace Raze.Script.Core.Exceptions.InterpreterExceptions;

public abstract class IntepreterException : RazeException
{
    public IntepreterException(string error, int line, int column, string className)
        : base(error, line, column, className)
    {
    }
}
