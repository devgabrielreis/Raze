namespace Raze.Script.Core.Tokens.Literals;

internal class IntegerLiteral : LiteralToken
{
    public IntegerLiteral(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
