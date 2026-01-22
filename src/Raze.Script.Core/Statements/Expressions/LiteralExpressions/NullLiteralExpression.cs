using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal class NullLiteralExpression : LiteralExpression
{
    public override object? Value => null;
    public NullLiteralExpression(SourceInfo source)
        : base(source)
    {
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitNullLiteralExpression(this, state);
    }
}
