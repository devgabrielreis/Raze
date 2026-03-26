using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class UnexpectedReturnType : RuntimeException
{
    internal UnexpectedReturnType(string message, SourceInfo source)
        : base(message, source, nameof(UnexpectedReturnType))
    {
    }
}
