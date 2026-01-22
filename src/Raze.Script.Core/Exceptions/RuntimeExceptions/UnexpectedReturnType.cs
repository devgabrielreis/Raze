using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class UnexpectedReturnType : RuntimeException
{
    internal UnexpectedReturnType(string message, SourceInfo source)
        : base(message, source, nameof(UnexpectedReturnType))
    {
    }
}
