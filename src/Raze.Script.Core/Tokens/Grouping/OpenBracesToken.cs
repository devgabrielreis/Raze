using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Grouping;

internal class OpenBracesToken : GroupingToken
{
    public OpenBracesToken(string lexeme, SourceInfo source)
    : base(lexeme, source)
    {
    }
}
