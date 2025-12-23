using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators;

internal class CompoundAssignmentToken : AssignmentToken
{
    internal string Operator { get; private set; }

    internal CompoundAssignmentToken(string op, string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
        Operator = op;
    }
}
