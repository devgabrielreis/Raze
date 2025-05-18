namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class UnsupportedStatementException : RuntimeException
{
    public UnsupportedStatementException(string statementType, int line, int column)
        : base($"Not supported statement found on interpreter: {statementType}", line, column, nameof(UnsupportedStatementException))
    {
    }
}
