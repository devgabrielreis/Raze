using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators;

internal class DecrementToken : OperatorToken
{
    internal DecrementToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
