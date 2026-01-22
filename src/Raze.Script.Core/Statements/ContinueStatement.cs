using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal class ContinueStatement : Statement
{
    public ContinueStatement(SourceInfo source)
        : base(source)
    {
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitContinueStatement(this, state);
    }
}
