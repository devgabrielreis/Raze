using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators.RelationalOperators;

internal abstract class RelationalOperatorToken : OperatorToken
{
    public RelationalOperatorToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
