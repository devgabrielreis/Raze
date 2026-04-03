using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class CorruptedContextStackException : RuntimeException
{
    internal CorruptedContextStackException(string message, SourceInfo source)
        : base(message, source, nameof(CorruptedContextStackException))
    {
    }
}
