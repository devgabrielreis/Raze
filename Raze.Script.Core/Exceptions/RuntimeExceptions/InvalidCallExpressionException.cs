using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class InvalidCallExpressionException : RuntimeException
{
    internal InvalidCallExpressionException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidCallExpressionException))
    {
    }
}
