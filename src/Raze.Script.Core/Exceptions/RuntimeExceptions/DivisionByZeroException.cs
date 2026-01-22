using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class DivisionByZeroException : RuntimeException
{
    internal DivisionByZeroException(SourceInfo source)
        : base(source, nameof(DivisionByZeroException))
    {
    }
}
