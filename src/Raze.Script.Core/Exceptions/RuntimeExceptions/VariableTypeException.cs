using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class VariableTypeException : RuntimeException
{
    internal VariableTypeException(string message, SourceInfo source)
        : base(message, source, nameof(VariableTypeException))
    {
    }
}
