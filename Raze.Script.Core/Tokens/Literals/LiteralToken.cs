using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Literals;

internal abstract class LiteralToken : Token
{
    public LiteralToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
