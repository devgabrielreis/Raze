using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class DivisionByZeroException
    : RuntimeException, IThrowableByThrowHelper<DivisionByZeroException>
{
    private DivisionByZeroException(string message, SourceInfo source)
        : base(message, source, nameof(DivisionByZeroException))
    {
    }

    static DivisionByZeroException IThrowableByThrowHelper<DivisionByZeroException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new DivisionByZeroException(message, source);
    }
}
