using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public abstract class RuntimeException : RazeException
{
    internal RuntimeException(string message, SourceInfo source, string errorName)
        : base(message, source, errorName)
    {
    }
}
