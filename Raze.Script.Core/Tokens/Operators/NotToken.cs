using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators;

internal class NotToken : OperatorToken
{
    internal NotToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
