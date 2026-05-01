using Raze.Script.Core.Metadata;
using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal sealed class AssignmentStatement : Statement
{
    internal readonly Expression Target;
    internal readonly Expression Value;

    internal AssignmentStatement(Expression target, Expression value, ref readonly SourceInfo source)
        : base(in source, true)
    {
        Target = target;
        Value = value;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitAssignmentStatement(this, state, out result);
    }
}
