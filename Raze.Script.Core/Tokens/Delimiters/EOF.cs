namespace Raze.Script.Core.Tokens.Delimiters;

internal class EOF : DelimiterToken
{
    public EOF(int line, int column)
        : base(string.Empty, line, column)
    {
    }
}
