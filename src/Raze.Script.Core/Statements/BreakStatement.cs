using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal sealed class BreakStatement : Statement
{
    internal BreakStatement(ref readonly SourceInfo source)
        : base(in source, true)
    {
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitBreakStatement(this, state, out result);
    }
}
