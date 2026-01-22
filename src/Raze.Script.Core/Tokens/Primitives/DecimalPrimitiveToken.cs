using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Primitives;

internal class DecimalPrimitiveToken : PrimitiveTypeToken
{
    public DecimalPrimitiveToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
