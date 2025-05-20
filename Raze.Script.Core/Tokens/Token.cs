namespace Raze.Script.Core.Tokens;

internal abstract class Token
{
    public string Lexeme { get; private set; }
    public int Line { get; private set; }
    public int Column { get; private set; }

    public Token(string lexeme, int line, int column)
    {
        Lexeme = lexeme;
        Line = line;
        Column = column;
    }
}
