namespace Raze.Script.Core.Tokens;

public class Token
{
    public TokenType TokenType { get; private set; }
    public string Lexeme { get; private set; }
    public int Line { get; private set; }
    public int Column { get; private set; }

    public Token(TokenType tokenType, string lexeme, int line, int column)
    {
        TokenType = tokenType;
        Lexeme = lexeme;
        Line = line;
        Column = column;
    }

    public override string ToString()
    {
        if (TokenType == TokenType.EOF)
        {
            return $"{TokenType.ToString()}, Line: {Line}, Column: {Column}";
        }
        else
        {
            return $"{TokenType.ToString()}: {Lexeme}, Line: {Line}, Column: {Column}";
        }
    }
}
