using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.LexerExceptions;

public abstract class LexerException : RazeException
{
    internal LexerException(string error, SourceInfo source, string errorName)
        : base(error, source, errorName)
    {
    }
}
