using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal sealed class NullLiteralExpression : LiteralExpression
{
    internal NullLiteralExpression(SourceInfo source)
        : base(source, true)
    {
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitNullLiteralExpression(this, state);
    }
}
