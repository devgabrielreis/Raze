namespace Raze.Script.Core.Tokens.Literals;

internal abstract class LiteralToken : Token
{
    public LiteralToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
