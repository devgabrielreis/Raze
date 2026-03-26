using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal sealed class CallExpression : Expression
{
    internal Expression Caller { get; private set; }

    internal IReadOnlyList<Expression> ArgumentList { get; private set; }

    internal CallExpression(Expression caller, IReadOnlyList<Expression> argumentList, SourceInfo source)
        : base(source, true)
    {
        Caller = caller;
        ArgumentList = argumentList;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitCallExpression(this, state);
    }
}
