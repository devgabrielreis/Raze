namespace Raze.Script.Core.Tokens.Literals;

internal class IntegerLiteralToken : LiteralToken
{
    public IntegerLiteralToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
