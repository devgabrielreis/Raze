using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.LexerExceptions;

public class InvalidStringException : LexerException
{
    internal InvalidStringException(string stringToken, SourceInfo source)
        : base($"Invalid string declaration: {stringToken}", source, nameof(InvalidStringException))
    {
    }
}
