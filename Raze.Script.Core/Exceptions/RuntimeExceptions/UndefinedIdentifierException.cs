using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class UndefinedIdentifierException : RuntimeException
{
    internal UndefinedIdentifierException(string symbol, SourceInfo source)
        : base($"Undefined identifier: {symbol}", source, nameof(UndefinedIdentifierException))
    {
    }
}
