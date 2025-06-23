namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class UninitializedVariableException : RuntimeException
{
    public UninitializedVariableException(int? line, int? column)
        : base($"Trying to access variable before its initialization", line, column, nameof(UninitializedVariableException))
    {
    }
}
