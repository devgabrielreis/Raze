using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class ScopeDeclarationException
    : RuntimeException, IThrowableByThrowHelper<ScopeDeclarationException>
{
    private ScopeDeclarationException(string message, SourceInfo source)
        : base(message, source, nameof(ScopeDeclarationException))
    {
    }

    static ScopeDeclarationException IThrowableByThrowHelper<ScopeDeclarationException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new ScopeDeclarationException(message, source);
    }
}
