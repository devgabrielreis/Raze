namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class InvalidAssignmentException : RuntimeException
{
    public InvalidAssignmentException(int? line, int? column)
        : base(line, column, nameof(InvalidAssignmentException))
    {
    }
}
