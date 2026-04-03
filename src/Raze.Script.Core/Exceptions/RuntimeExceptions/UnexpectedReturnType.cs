using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class UnexpectedReturnType
    : RuntimeException, IThrowableByThrowHelper<UnexpectedReturnType>
{
    private UnexpectedReturnType(string message, SourceInfo source)
        : base(message, source, nameof(UnexpectedReturnType))
    {
    }

    static UnexpectedReturnType IThrowableByThrowHelper<UnexpectedReturnType>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new UnexpectedReturnType(message, source);
    }
}
