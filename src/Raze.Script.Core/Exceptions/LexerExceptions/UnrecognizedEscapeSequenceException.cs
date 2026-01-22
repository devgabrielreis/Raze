using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.LexerExceptions;

public class UnrecognizedEscapeSequenceException : LexerException
{
    internal UnrecognizedEscapeSequenceException(string escapeSequence, SourceInfo source)
        : base(escapeSequence, source, nameof(UnrecognizedEscapeSequenceException))
    {
    }
}
