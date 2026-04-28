using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal sealed class BooleanLiteralExpression : LiteralExpression
{
    internal readonly bool BoolValue;

    internal BooleanLiteralExpression(bool value, ref readonly SourceInfo source)
        : base(in source, true)
    {
        BoolValue = value;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitBooleanLiteralExpression(this, state, out result);
    }
}
