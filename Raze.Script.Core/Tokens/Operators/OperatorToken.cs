using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators;

internal abstract class OperatorToken : Token
{
    public OperatorToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
