namespace Raze.Script.Core.Exceptions.ControlExceptions;

internal abstract class ControlException : RazeException
{
    public ControlException(string className)
        : base(null, null, className)
    {
    }
}
