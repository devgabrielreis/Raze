namespace Raze.Script.Core.Tokens.Literals;

internal class StringLiteral : LiteralToken
{
    public StringLiteral(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
