using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators.AdditiveOperators;

internal class AdditionToken : AdditiveOperatorToken
{
    public AdditionToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
