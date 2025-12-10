using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators;

internal class QuestionMarkToken : OperatorToken
{
    public QuestionMarkToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
