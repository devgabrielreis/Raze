using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class InvalidCallExpressionException
    : RuntimeException, IThrowableByThrowHelper<InvalidCallExpressionException>
{
    private InvalidCallExpressionException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidCallExpressionException))
    {
    }

    static InvalidCallExpressionException IThrowableByThrowHelper<InvalidCallExpressionException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new InvalidCallExpressionException(message, source);
    }
}
