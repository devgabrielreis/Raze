using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal sealed class DecimalLiteralExpression : LiteralExpression
{
    internal readonly decimal DecValue;

    internal DecimalLiteralExpression(decimal value, SourceInfo source)
        : base(source, true)
    {
        DecValue = value;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitDecimalLiteralExpression(this, state);
    }
}
