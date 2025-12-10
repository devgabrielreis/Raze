using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Primitives;

internal abstract class PrimitiveTypeToken : Token
{
    public PrimitiveTypeToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
