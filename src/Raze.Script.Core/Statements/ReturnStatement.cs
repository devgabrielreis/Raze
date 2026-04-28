using Raze.Script.Core.Metadata;
using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal sealed class ReturnStatement : Statement
{
    internal readonly Expression? ReturnedValue;

    internal ReturnStatement(Expression? returnedValue, ref readonly SourceInfo source)
        : base(in source, true)
    {
        ReturnedValue = returnedValue;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitReturnStatement(this, state, out result);
    }
}
