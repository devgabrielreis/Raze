using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Delimiters;

internal class CommaToken : DelimiterToken
{
    public CommaToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
