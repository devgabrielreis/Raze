namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class DivisionByZeroException : RuntimeException
{
    public DivisionByZeroException(int line, int column)
        : base(line, column, nameof(DivisionByZeroException))
    {
    }
}
