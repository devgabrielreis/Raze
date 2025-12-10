using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators.EqualityOperators;

internal abstract class EqualityOperatorToken : OperatorToken
{
    public EqualityOperatorToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
