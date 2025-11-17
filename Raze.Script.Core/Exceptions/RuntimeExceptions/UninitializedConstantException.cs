namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class UninitializedConstantException : RuntimeException
{
    public UninitializedConstantException(int? line, int? column)
        : base($"Cannot declare a constant without an initializer", line, column, nameof(UninitializedConstantException))
    {
    }
}
