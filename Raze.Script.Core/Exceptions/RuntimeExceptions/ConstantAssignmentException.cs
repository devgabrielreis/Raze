namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class ConstantAssignmentException : RuntimeException
{
    public ConstantAssignmentException(int? line, int? column)
        : base("Cannot modify a constant value", line, column, nameof(ConstantAssignmentException))
    {
    }
}
