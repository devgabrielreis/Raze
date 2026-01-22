using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions;

public abstract class RazeException : Exception
{
    public SourceInfo SourceInfo { get; private set; }

    internal RazeException(string error, SourceInfo source, string errorName)
        : base($"{errorName} -> {error}.")
    {
        SourceInfo = source;
    }

    internal RazeException(SourceInfo source, string errorName)
        : base($"{errorName}.")
    {
        SourceInfo = source;
    }
}
