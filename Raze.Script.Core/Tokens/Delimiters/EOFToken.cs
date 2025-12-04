namespace Raze.Script.Core.Tokens.Delimiters;

internal class EOFToken : DelimiterToken
{
    public EOFToken(int line, int column)
        : base(string.Empty, line, column)
    {
    }
}
