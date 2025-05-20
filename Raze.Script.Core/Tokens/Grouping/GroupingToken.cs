namespace Raze.Script.Core.Tokens.Grouping;

internal abstract class GroupingToken : Token
{
    public GroupingToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
