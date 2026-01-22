using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators.MultiplicativeOperators;

internal abstract class MultiplicativeOperatorToken : OperatorToken
{
    public MultiplicativeOperatorToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
