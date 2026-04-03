using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class ConstantAssignmentException
    : RuntimeException, IThrowableByThrowHelper<ConstantAssignmentException>
{
    private ConstantAssignmentException(string message, SourceInfo source)
        : base(message, source, nameof(ConstantAssignmentException))
    {
    }

    static ConstantAssignmentException IThrowableByThrowHelper<ConstantAssignmentException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new ConstantAssignmentException(message, source);
    }
}
