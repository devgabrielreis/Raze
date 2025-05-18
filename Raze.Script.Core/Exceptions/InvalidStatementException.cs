namespace Raze.Script.Core.Exceptions;

public class InvalidStatementException : ParseException
{
    public InvalidStatementException(string statementType, int line, int column)
        : base($"Not supported statement found on interpreter: {statementType}", line, column, nameof(InvalidStatementException))
    {
    }
}
