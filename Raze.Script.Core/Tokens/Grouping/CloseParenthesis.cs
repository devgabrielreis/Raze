namespace Raze.Script.Core.Tokens.Grouping;

internal class CloseParenthesis : GroupingToken
{
    public CloseParenthesis(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
