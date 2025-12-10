using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Grouping;

internal abstract class GroupingToken : Token
{
    public GroupingToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
