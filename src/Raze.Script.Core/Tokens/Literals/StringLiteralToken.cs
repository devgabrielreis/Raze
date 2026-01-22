using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Literals;

internal class StringLiteralToken : LiteralToken
{
    public StringLiteralToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
