using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class UninitializedConstantException
    : RuntimeException, IThrowableByThrowHelper<UninitializedConstantException>
{
    private UninitializedConstantException(string message, SourceInfo source)
        : base(message, source, nameof(UninitializedConstantException))
    {
    }

    static UninitializedConstantException IThrowableByThrowHelper<UninitializedConstantException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new UninitializedConstantException(message, source);
    }
}
