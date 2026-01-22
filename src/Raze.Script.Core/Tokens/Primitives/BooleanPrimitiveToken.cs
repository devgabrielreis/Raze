using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Primitives;

internal class BooleanPrimitiveToken : PrimitiveTypeToken
{
    public BooleanPrimitiveToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
