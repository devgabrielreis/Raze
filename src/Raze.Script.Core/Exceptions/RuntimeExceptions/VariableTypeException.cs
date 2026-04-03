using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class VariableTypeException
    : RuntimeException, IThrowableByThrowHelper<VariableTypeException>
{
    private VariableTypeException(string message, SourceInfo source)
        : base(message, source, nameof(VariableTypeException))
    {
    }

    static VariableTypeException IThrowableByThrowHelper<VariableTypeException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new VariableTypeException(message, source);
    }
}
