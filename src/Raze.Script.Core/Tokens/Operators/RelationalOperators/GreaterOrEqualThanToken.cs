using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators.RelationalOperators;

internal class GreaterOrEqualThanToken : RelationalOperatorToken
{
    public GreaterOrEqualThanToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
