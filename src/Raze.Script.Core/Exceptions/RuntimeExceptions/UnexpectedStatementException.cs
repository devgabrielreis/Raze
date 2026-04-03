using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class UnexpectedStatementException
    : RuntimeException, IThrowableByThrowHelper<UnexpectedStatementException>
{
    private UnexpectedStatementException(string message, SourceInfo source)
        : base(message, source, nameof(UnexpectedStatementException))
    {
    }

    static UnexpectedStatementException IThrowableByThrowHelper<UnexpectedStatementException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new UnexpectedStatementException(message, source);
    }
}
