using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal sealed class ContinueStatement : Statement
{
    internal ContinueStatement(ref readonly SourceInfo source)
        : base(in source, true)
    {
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitContinueStatement(this, state, out result);
    }
}
