using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators;

internal class NullCheckerToken : OperatorToken
{
    internal NullCheckerToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
