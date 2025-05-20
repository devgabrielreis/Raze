namespace Raze.Script.Core.Exceptions.ParseExceptions;

public class InvalidTokenListException : ParseException
{
    public InvalidTokenListException()
        : base("Invalid token list", null, null, nameof(InvalidTokenListException))
    {
    }
}
