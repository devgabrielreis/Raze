using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Literals;

internal class BooleanLiteralToken : LiteralToken
{
    public BooleanLiteralToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
