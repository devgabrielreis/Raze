using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class UndefinedIdentifierException
    : RuntimeException, IThrowableByThrowHelper<UndefinedIdentifierException>
{
    private UndefinedIdentifierException(string message, SourceInfo source)
        : base(message, source, nameof(UndefinedIdentifierException))
    {
    }

    static UndefinedIdentifierException IThrowableByThrowHelper<UndefinedIdentifierException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new UndefinedIdentifierException(message, source);
    }
}
