using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public abstract class RuntimeException : RazeException
{
    internal RuntimeException(string error, SourceInfo source, string errorName)
        : base(error, source, errorName)
    {
    }

    internal RuntimeException(SourceInfo source, string errorName)
        : base(source, errorName)
    {
    }
}
