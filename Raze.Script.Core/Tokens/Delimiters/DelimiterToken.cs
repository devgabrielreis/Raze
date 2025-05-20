namespace Raze.Script.Core.Tokens.Delimiters;

internal abstract class DelimiterToken : Token
{
    public DelimiterToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
