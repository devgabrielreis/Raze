using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.InterpreterExceptions;

public class UnsupportedStatementException : InterpreterException
{
    internal UnsupportedStatementException(string statementType, SourceInfo source)
        : base($"Not supported statement found on interpreter: {statementType}", source, nameof(UnsupportedStatementException))
    {
    }
}
