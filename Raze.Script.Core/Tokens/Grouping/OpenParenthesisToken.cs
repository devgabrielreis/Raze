using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Grouping;

internal class OpenParenthesisToken : GroupingToken
{
    public OpenParenthesisToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
