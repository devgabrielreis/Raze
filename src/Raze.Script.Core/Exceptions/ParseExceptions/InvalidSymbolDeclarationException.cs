using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public sealed class InvalidSymbolDeclarationException
    : ParseException, IThrowableByThrowHelper<InvalidSymbolDeclarationException>
{
    private InvalidSymbolDeclarationException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidSymbolDeclarationException))
    {
    }

    static InvalidSymbolDeclarationException IThrowableByThrowHelper<InvalidSymbolDeclarationException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new InvalidSymbolDeclarationException(message, source);
    }
}
