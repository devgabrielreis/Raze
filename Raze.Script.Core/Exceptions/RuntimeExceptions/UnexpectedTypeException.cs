namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class UnexpectedTypeException : RuntimeException
{
    public UnexpectedTypeException(string foundType, string expectedType, int? line, int? column)
        : base($"Expected: {expectedType}. Found: {foundType}", line, column, nameof(UnexpectedTypeException))
    {
    }
}
