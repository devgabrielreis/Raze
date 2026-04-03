using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class UninitializedVariableException : RuntimeException
{
    internal UninitializedVariableException(string message, SourceInfo source)
        : base(message, source, nameof(UninitializedVariableException))
    {
    }
}
