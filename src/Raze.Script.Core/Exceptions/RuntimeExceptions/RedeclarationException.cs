using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class RedeclarationException
    : RuntimeException, IThrowableByThrowHelper<RedeclarationException>
{
    private RedeclarationException(string message, SourceInfo source)
        : base(message, source, nameof(RedeclarationException))
    {
    }

    static RedeclarationException IThrowableByThrowHelper<RedeclarationException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new RedeclarationException(message, source);
    }
}
