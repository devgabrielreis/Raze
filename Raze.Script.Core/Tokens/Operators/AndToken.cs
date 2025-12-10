using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators;

internal class AndToken : OperatorToken
{
    public AndToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
