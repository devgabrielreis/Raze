using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class UnsupportedBinaryOperationException : RuntimeException
{
    internal UnsupportedBinaryOperationException(string leftType, string rightType, string op, SourceInfo source)
        : base(
            $"{leftType} {op} {rightType}",
            source,
            nameof(UnsupportedBinaryOperationException)
        )
    {
    }
}
