namespace Raze.Script.Core.Tokens;

internal class IdentifierToken : Token
{
    public IdentifierToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
