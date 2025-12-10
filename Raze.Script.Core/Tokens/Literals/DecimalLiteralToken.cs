using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Literals;

internal class DecimalLiteralToken : LiteralToken
{
    public DecimalLiteralToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
