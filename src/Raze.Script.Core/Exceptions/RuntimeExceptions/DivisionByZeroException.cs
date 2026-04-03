using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class DivisionByZeroException : RuntimeException
{
    internal DivisionByZeroException(string message, SourceInfo source)
        : base(message, source, nameof(DivisionByZeroException))
    {
    }
}
