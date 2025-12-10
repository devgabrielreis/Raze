using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators.RelationalOperators;

internal class LessThanToken : RelationalOperatorToken
{
    public LessThanToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
