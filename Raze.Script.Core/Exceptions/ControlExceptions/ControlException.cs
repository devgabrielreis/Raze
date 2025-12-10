using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ControlExceptions;

internal abstract class ControlException : RazeException
{
    internal ControlException(SourceInfo source, string errorName)
        : base(source, errorName)
    {
    }
}
