using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal sealed class IntegerLiteralExpression : LiteralExpression
{
    internal readonly Int128 IntValue;

    internal IntegerLiteralExpression(Int128 value, ref readonly SourceInfo source)
        : base(in source, true)
    {
        IntValue = value;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitIntegerLiteralExpression(this, state, out result);
    }
}
