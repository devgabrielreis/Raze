namespace Raze.Script.Core.Exceptions.InterpreterExceptions;

public class UnsupportedStatementException : InterpreterException
{
    public UnsupportedStatementException(string statementType, int line, int column)
        : base($"Not supported statement found on interpreter: {statementType}", line, column, nameof(UnsupportedStatementException))
    {
    }
}
