using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators.RelationalOperators;

internal class LessOrEqualThanToken : RelationalOperatorToken
{
    public LessOrEqualThanToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
