namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class VariableTypeException : RuntimeException
{
    public VariableTypeException(string foundType, string variableType, int line,int column)
        : base($"Trying to assign type {foundType} to variable of type {variableType}", line, column, nameof(VariableTypeException))
    {
    }
}
