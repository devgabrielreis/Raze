using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.InterpreterExceptions;

public abstract class InterpreterException : RazeException
{
    internal InterpreterException(string error, SourceInfo source, string errorName)
        : base(error, source, errorName)
    {
    }
}
