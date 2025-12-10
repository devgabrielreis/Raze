using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Delimiters;

internal class EOFToken : DelimiterToken
{
    public EOFToken(SourceInfo source)
        : base(string.Empty, source)
    {
    }
}
