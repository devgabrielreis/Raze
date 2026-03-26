using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public sealed class InvalidTypeDeclarationException : ParseException
{
    internal InvalidTypeDeclarationException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidTypeDeclarationException))
    {
    }
}
