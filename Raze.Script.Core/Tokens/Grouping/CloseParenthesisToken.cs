namespace Raze.Script.Core.Tokens.Grouping;

internal class CloseParenthesisToken : GroupingToken
{
    public CloseParenthesisToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
