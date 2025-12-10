using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Delimiters;

internal class SemiColonToken : DelimiterToken
{
    public SemiColonToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
