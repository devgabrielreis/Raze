using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Literals;

internal class NullLiteralToken : LiteralToken
{
    public NullLiteralToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
