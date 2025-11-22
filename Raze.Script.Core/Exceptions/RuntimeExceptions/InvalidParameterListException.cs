namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class InvalidParameterListException : RuntimeException
{
    public InvalidParameterListException(string message, int? line, int? column)
        : base(message, line, column, nameof(InvalidParameterListException))
    {
    }
}
