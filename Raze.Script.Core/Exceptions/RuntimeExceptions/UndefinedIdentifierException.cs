namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class UndefinedIdentifierException : RuntimeException
{
    public UndefinedIdentifierException(string symbol, int? line, int? column)
        : base($"Undefined identifier: {symbol}", line, column, nameof(UndefinedIdentifierException))
    {
    }
}
