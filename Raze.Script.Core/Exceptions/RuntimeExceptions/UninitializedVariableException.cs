using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class UninitializedVariableException : RuntimeException
{
    internal UninitializedVariableException(SourceInfo source)
        : base($"Trying to access variable before its initialization", source, nameof(UninitializedVariableException))
    {
    }
}
