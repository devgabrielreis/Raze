namespace Raze.Script.Core.Exceptions;

public class NullValueException : RuntimeException
{
    public NullValueException(int line, int column)
        : base($"Unsupported operation with nullified value", line, column, nameof(NullValueException))
    {
    }
}
