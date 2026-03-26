using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class CorruptedContextStackException : RuntimeException
{
    internal CorruptedContextStackException(SourceInfo source)
        : base(source, nameof(CorruptedContextStackException))
    {
    }
}
