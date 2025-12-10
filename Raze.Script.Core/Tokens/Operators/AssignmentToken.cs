using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators;

internal class AssignmentToken : OperatorToken
{
    public AssignmentToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
