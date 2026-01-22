using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Literals;

internal class IntegerLiteralToken : LiteralToken
{
    public IntegerLiteralToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
