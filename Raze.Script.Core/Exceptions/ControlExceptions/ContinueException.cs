using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ControlExceptions;

internal class ContinueException : ControlException
{
    internal ContinueException(SourceInfo source)
        : base(source, nameof(ContinueException))
    {
    }
}
