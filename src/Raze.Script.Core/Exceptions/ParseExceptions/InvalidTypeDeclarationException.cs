using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public sealed class InvalidTypeDeclarationException
    : ParseException, IThrowableByThrowHelper<InvalidTypeDeclarationException>
{
    private InvalidTypeDeclarationException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidTypeDeclarationException))
    {
    }

    static InvalidTypeDeclarationException IThrowableByThrowHelper<InvalidTypeDeclarationException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new InvalidTypeDeclarationException(message, source);
    }
}
