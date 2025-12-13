using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Primitives;

internal class VoidPrimitiveToken : PrimitiveTypeToken
{
    public VoidPrimitiveToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
