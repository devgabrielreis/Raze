using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public abstract class ParseException : RazeException
{
    internal ParseException(string error, SourceInfo source, string errorName)
        : base(error, source, errorName)
    {
    }
}
