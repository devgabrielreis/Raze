using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class UnexpectedStatementException : RuntimeException
{
    internal UnexpectedStatementException(string error, SourceInfo source)
        : base(error, source, nameof(UnexpectedStatementException))
    {
    }
}
