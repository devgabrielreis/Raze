using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Primitives;

internal class IntegerPrimitiveToken : PrimitiveTypeToken
{
    public IntegerPrimitiveToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
