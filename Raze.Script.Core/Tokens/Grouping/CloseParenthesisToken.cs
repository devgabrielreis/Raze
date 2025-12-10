using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Grouping;

internal class CloseParenthesisToken : GroupingToken
{
    public CloseParenthesisToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
