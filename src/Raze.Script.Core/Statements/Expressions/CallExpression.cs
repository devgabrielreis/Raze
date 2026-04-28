using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal sealed class CallExpression : Expression
{
    internal readonly Expression Caller;

    internal readonly IReadOnlyList<Expression> ArgumentList;

    internal CallExpression(Expression caller, IReadOnlyList<Expression> argumentList, ref readonly SourceInfo source)
        : base(in source, true)
    {
        Caller = caller;
        ArgumentList = argumentList;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitCallExpression(this, state, out result);
    }
}
