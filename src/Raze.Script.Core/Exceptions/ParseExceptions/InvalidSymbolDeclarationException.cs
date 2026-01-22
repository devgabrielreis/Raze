using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public class InvalidSymbolDeclarationException : ParseException
{
    internal InvalidSymbolDeclarationException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidSymbolDeclarationException))
    {
    }
}
