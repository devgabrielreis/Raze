namespace Raze.Script.Core.Exceptions.ControlExceptions;

internal class BreakException : ControlException
{
    public BreakException()
        : base(nameof(BreakException))
    {
    }
}
