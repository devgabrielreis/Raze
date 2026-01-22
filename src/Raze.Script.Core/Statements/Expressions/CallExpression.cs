using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal class CallExpression : Expression
{
    public Expression Caller { get; private set; }

    public IReadOnlyList<Expression> ArgumentList { get; private set; }

    public CallExpression(Expression caller, IReadOnlyList<Expression> argumentList, SourceInfo source)
        : base(source)
    {
        Caller = caller;
        ArgumentList = argumentList;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitCallExpression(this, state);
    }
}
