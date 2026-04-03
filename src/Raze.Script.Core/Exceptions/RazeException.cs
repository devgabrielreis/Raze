using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions;

public abstract class RazeException : Exception
{
    public SourceInfo SourceInfo { get; private set; }

    internal RazeException(string message, SourceInfo source, string errorName)
        : base($"{errorName} -> {message}.")
    {
        SourceInfo = source;
    }
}
