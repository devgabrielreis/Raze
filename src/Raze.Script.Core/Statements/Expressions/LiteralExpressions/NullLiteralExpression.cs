using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal sealed class NullLiteralExpression : LiteralExpression
{
    internal NullLiteralExpression(ref readonly SourceInfo source)
        : base(in source, true)
    {
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitNullLiteralExpression(this, state, out result);
    }
}
