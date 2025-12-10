using Raze.Script.Core.Metadata;
using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal class AssignmentStatement : Statement
{
    public Expression Target { get; private set; }
    public Expression Value { get; private set; }

    public AssignmentStatement(Expression target, Expression value, SourceInfo source)
        : base(source)
    {
        Target = target;
        Value = value;
    }
}
