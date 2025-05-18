namespace Raze.Script.Core.Exceptions;

public class InvalidExpressionException : ParseException
{
    public InvalidExpressionException(string error, int line, int column)
        : base(error, line, column, nameof(InvalidExpressionException))
    {
    }
}
