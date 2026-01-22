using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ControlExceptions;

internal class BreakException : ControlException
{
    internal BreakException(SourceInfo source)
        : base(source, nameof(BreakException))
    {
    }
}
