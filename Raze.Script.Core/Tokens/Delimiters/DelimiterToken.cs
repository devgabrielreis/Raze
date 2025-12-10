using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Delimiters;

internal abstract class DelimiterToken : Token
{
    public DelimiterToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
