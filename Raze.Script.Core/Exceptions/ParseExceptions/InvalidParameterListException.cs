namespace Raze.Script.Core.Exceptions.ParseExceptions;

public class InvalidParameterListException : ParseException
{
    public InvalidParameterListException(string message, int? line, int? column)
        : base(message, line, column, nameof(InvalidParameterListException))
    {
    }
}
