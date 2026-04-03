using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public sealed class InvalidTokenListException : ParseException
{
    internal InvalidTokenListException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidTokenListException))
    {
    }
}
