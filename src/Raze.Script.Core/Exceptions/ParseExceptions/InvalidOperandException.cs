using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public sealed class InvalidOperandException
    : ParseException, IThrowableByThrowHelper<InvalidOperandException>
{
    private InvalidOperandException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidOperandException))
    {
    }

    static InvalidOperandException IThrowableByThrowHelper<InvalidOperandException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new InvalidOperandException(message, source);
    }
}
