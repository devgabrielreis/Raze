using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators;

internal class CompoundAssignmentToken : AssignmentToken
{
    internal OperatorToken Operator { get; private set; }

    internal CompoundAssignmentToken(OperatorToken op, string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
        Operator = op;
    }
}
