namespace Raze.Script.Core.Tokens.Literals;

internal class NullLiteral : LiteralToken
{
    public NullLiteral(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
