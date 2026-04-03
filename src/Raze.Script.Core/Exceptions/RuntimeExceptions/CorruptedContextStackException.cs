using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class CorruptedContextStackException
    : RuntimeException, IThrowableByThrowHelper<CorruptedContextStackException>
{
    private CorruptedContextStackException(string message, SourceInfo source)
        : base(message, source, nameof(CorruptedContextStackException))
    {
    }

    static CorruptedContextStackException IThrowableByThrowHelper<CorruptedContextStackException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new CorruptedContextStackException(message, source);
    }
}
