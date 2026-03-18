using Raze.Script.Core.Exceptions;

namespace Raze.Script.Core.Result;

public class RazeError : RazeResult
{
    public RazeException Error { get; }

    internal RazeError(RazeException error)
        : base(false)
    {
        Error = error;
    }
}
