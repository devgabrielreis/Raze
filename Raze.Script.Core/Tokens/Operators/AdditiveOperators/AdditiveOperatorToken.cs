using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators.AdditiveOperators;

internal abstract class AdditiveOperatorToken : OperatorToken
{
    public AdditiveOperatorToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
