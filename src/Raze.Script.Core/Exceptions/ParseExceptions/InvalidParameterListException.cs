using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public class InvalidParameterListException : ParseException
{
    internal InvalidParameterListException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidParameterListException))
    {
    }
}
