using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public sealed class UnexpectedTokenException : ParseException
{
    internal UnexpectedTokenException(string message, SourceInfo source)
        : base(message, source, nameof(UnexpectedTokenException))
    {
    }
}
