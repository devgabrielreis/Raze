using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators.EqualityOperators;

internal class EqualToken : EqualityOperatorToken
{
    public EqualToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
