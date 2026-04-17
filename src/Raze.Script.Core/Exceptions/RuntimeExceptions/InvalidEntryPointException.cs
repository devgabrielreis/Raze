using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class InvalidEntryPointException : RuntimeException
{
    internal InvalidEntryPointException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidEntryPointException))
    {
    }
}
