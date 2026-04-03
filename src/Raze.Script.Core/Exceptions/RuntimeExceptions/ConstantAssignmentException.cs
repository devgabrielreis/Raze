using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class ConstantAssignmentException : RuntimeException
{
    internal ConstantAssignmentException(string message, SourceInfo source)
        : base(message, source, nameof(ConstantAssignmentException))
    {
    }
}
