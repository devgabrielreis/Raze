using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal sealed class DecimalLiteralExpression : LiteralExpression
{
    internal readonly decimal DecValue;

    internal DecimalLiteralExpression(decimal value, ref readonly SourceInfo source)
        : base(in source, true)
    {
        DecValue = value;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitDecimalLiteralExpression(this, state, out result);
    }
}
