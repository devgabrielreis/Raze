using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class UninitializedVariableException
    : RuntimeException, IThrowableByThrowHelper<UninitializedVariableException>
{
    private UninitializedVariableException(string message, SourceInfo source)
        : base(message, source, nameof(UninitializedVariableException))
    {
    }

    static UninitializedVariableException IThrowableByThrowHelper<UninitializedVariableException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new UninitializedVariableException(message, source);
    }
}
