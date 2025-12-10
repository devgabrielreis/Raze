using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Grouping;

internal class CloseBracesToken : GroupingToken
{
    public CloseBracesToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
