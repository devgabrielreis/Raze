using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators.MultiplicativeOperators;

internal class MultiplicationToken : MultiplicativeOperatorToken
{
    public MultiplicationToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
