using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.LexerExceptions;

public abstract class LexerException : RazeException
{
    internal LexerException(string message, SourceInfo source, string errorName)
        : base(message, source, errorName)
    {
    }
}
