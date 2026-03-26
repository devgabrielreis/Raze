using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ControlExceptions;

internal sealed class ContinueException : ControlException
{
    internal ContinueException(SourceInfo source)
        : base(source, nameof(ContinueException))
    {
    }
}
