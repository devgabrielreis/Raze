using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal sealed class BreakStatement : Statement
{
    internal BreakStatement(SourceInfo source)
        : base(source, true)
    {
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitBreakStatement(this, state);
    }
}
