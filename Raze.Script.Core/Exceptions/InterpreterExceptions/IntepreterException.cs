namespace Raze.Script.Core.Exceptions.InterpreterExceptions;

public class IntepreterException : RazeException
{
    public IntepreterException(string error, int line, int column, string className)
        : base(error, line, column, className)
    {
    }
}
