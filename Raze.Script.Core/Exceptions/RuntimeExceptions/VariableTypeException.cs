using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class VariableTypeException : RuntimeException
{
    internal VariableTypeException(string foundType, string variableType, SourceInfo source)
        : base($"Trying to assign type {foundType} to variable of type {variableType}", source, nameof(VariableTypeException))
    {
    }
}
