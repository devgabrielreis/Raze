using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class UnexpectedStatementException : RuntimeException
{
    internal UnexpectedStatementException(string message, SourceInfo source)
        : base(message, source, nameof(UnexpectedStatementException))
    {
    }
}
