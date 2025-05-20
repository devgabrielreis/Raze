using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal class AssignmentStatement : Statement
{
    public Expression Target { get; private set; }
    public Expression Value { get; private set; }

    public AssignmentStatement(Expression target, Expression value, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        Target = target;
        Value = value;
    }
}
