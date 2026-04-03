using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public abstract class ParseException : RazeException
{
    internal ParseException(string message, SourceInfo source, string errorName)
        : base(message, source, errorName)
    {
    }
}
