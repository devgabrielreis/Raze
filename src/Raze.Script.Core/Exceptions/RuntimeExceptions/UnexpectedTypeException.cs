using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class UnexpectedTypeException : RuntimeException
{
    internal UnexpectedTypeException(string message, SourceInfo source)
        : base(message, source, nameof(UnexpectedTypeException))
    {
    }
}
