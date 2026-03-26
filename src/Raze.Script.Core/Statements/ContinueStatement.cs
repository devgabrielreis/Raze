using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal sealed class ContinueStatement : Statement
{
    internal ContinueStatement(SourceInfo source)
        : base(source, true)
    {
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitContinueStatement(this, state);
    }
}
