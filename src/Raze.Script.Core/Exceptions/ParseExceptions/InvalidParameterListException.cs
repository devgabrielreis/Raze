using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public sealed class InvalidParameterListException
    : ParseException, IThrowableByThrowHelper<InvalidParameterListException>
{
    private InvalidParameterListException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidParameterListException))
    {
    }

    static InvalidParameterListException IThrowableByThrowHelper<InvalidParameterListException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new InvalidParameterListException(message, source);
    }
}
