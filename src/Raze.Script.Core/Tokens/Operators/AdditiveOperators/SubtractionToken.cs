using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators.AdditiveOperators;

internal class SubtractionToken : AdditiveOperatorToken
{
    public SubtractionToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
