using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators;

internal class IncrementToken : OperatorToken
{
    internal IncrementToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
