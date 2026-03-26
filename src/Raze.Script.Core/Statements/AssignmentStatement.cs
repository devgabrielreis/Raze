using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal sealed class AssignmentStatement : Statement
{
    internal IdentifierExpression Target { get; private set; }
    internal Expression Value { get; private set; }

    internal AssignmentStatement(IdentifierExpression target, Expression value, SourceInfo source)
        : base(source, true)
    {
        Target = target;
        Value = value;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitAssignmentStatement(this, state);
    }
}
