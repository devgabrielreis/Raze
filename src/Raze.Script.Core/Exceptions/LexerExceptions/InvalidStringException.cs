using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.LexerExceptions;

public sealed class InvalidStringException : LexerException
{
    internal InvalidStringException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidStringException))
    {
    }
}
