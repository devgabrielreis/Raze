namespace Raze.Script.Core.Result;

public abstract class RazeResult
{
    public bool Success { get; }

    internal RazeResult(bool success)
    {
        Success = success;
    }
}
