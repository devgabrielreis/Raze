using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class ConstantAssignmentException : RuntimeException
{
    internal ConstantAssignmentException(SourceInfo source)
        : base("Cannot modify a constant value", source, nameof(ConstantAssignmentException))
    {
    }
}
