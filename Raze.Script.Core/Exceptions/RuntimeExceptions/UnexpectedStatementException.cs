namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class UnexpectedStatementException : RuntimeException
{
    public UnexpectedStatementException(string error, int? line, int? column)
        : base(error, line, column, nameof(UnexpectedStatementException))
    {
    }
}
