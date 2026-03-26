using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal sealed class ReturnStatement : Statement
{
    internal Expression? ReturnedValue { get; private set; }

    internal ReturnStatement(Expression? returnedValue, SourceInfo source)
        : base(source, true)
    {
        ReturnedValue = returnedValue;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitReturnStatement(this, state);
    }
}
