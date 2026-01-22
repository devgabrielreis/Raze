using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators.EqualityOperators;

internal class NotEqualToken : EqualityOperatorToken
{
    public NotEqualToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
