using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class UnexpectedTypeException : RuntimeException
{
    internal UnexpectedTypeException(string foundType, string expectedType, SourceInfo source)
        : base($"Expected: {expectedType}. Found: {foundType}", source, nameof(UnexpectedTypeException))
    {
    }
}
