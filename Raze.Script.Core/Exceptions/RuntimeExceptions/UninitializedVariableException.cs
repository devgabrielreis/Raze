namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class UninitializedVariableException : RuntimeException
{
    public UninitializedVariableException(string varName, int? line, int? column, string className)
        : base($"Trying to access variable before its initialization: {varName}", line, column, nameof(UninitializedVariableException))
    {
    }
}
