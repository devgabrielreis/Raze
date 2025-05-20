namespace Raze.Script.Core.Tokens;

internal class Identifier : Token
{
    public Identifier(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
