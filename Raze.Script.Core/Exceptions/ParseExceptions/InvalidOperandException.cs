using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public class InvalidOperandException : ParseException
{
    internal InvalidOperandException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidOperandException))
    {
    }
}
