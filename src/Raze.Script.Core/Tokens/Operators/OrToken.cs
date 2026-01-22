using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators;

internal class OrToken : OperatorToken
{
    public OrToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
