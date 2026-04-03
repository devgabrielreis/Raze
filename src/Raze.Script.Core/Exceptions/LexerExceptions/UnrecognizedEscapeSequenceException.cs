using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.LexerExceptions;

public sealed class UnrecognizedEscapeSequenceException : LexerException
{
    internal UnrecognizedEscapeSequenceException(string message, SourceInfo source)
        : base(message, source, nameof(UnrecognizedEscapeSequenceException))
    {
    }
}
