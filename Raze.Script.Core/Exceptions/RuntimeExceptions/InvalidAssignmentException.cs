using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class InvalidAssignmentException : RuntimeException
{
    internal InvalidAssignmentException(SourceInfo source)
        : base(source, nameof(InvalidAssignmentException))
    {
    }
}
