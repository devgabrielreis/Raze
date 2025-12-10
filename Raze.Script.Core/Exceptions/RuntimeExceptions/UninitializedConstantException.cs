using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class UninitializedConstantException : RuntimeException
{
    internal UninitializedConstantException(SourceInfo source)
        : base($"Cannot declare a constant without an initializer", source, nameof(UninitializedConstantException))
    {
    }
}
