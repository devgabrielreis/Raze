using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators.RelationalOperators;

internal class GreaterThanToken : RelationalOperatorToken
{
    public GreaterThanToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
