using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public sealed class InvalidTokenListException : ParseException
{
    internal InvalidTokenListException(SourceInfo source)
        : base("Invalid token list", source, nameof(InvalidTokenListException))
    {
    }
}
