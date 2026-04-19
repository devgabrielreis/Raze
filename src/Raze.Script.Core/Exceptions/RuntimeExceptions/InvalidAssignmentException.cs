using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class InvalidAssignmentException
    : RuntimeException, IThrowableByThrowHelper<InvalidAssignmentException>
{
    private InvalidAssignmentException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidAssignmentException))
    {
    }

    static InvalidAssignmentException IThrowableByThrowHelper<InvalidAssignmentException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new InvalidAssignmentException(message, source);
    }
}
