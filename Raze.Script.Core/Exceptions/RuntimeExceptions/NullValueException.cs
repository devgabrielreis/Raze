namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class NullValueException : RuntimeException
{
    public NullValueException(int line, int column)
        : base($"Unsupported operation with nullified value", line, column, nameof(NullValueException))
    {
    }
}
