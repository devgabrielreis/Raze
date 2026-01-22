using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens;

internal class IdentifierToken : Token
{
    public IdentifierToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
