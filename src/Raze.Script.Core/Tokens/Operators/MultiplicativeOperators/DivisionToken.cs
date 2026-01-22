using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators.MultiplicativeOperators;

internal class DivisionToken : MultiplicativeOperatorToken
{
    public DivisionToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
