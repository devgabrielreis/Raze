namespace Raze.Script.Core.Tokens.Literals;

internal class StringLiteralToken : LiteralToken
{
    public StringLiteralToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
