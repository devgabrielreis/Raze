using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public sealed class UnsupportedUnaryOperationException : RuntimeException
{
    internal UnsupportedUnaryOperationException(string op, string valueType, bool isPostFix, SourceInfo source)
        : base(
            isPostFix ? $"{valueType}{op}" : $"{op}{valueType}",
            source,
            nameof(UnsupportedUnaryOperationException)
        )
    {
    }
}
