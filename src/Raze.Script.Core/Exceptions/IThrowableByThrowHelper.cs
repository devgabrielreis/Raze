using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions;

public interface IThrowableByThrowHelper<TSelf>
    where TSelf : RazeException
{
    internal static abstract TSelf Create(string message, ref readonly SourceInfo source);
}
