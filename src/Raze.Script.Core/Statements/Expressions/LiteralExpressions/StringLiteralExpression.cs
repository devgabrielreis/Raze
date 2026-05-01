using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal sealed class StringLiteralExpression : LiteralExpression
{
    internal readonly string StrValue;

    internal StringLiteralExpression(string value, ref readonly SourceInfo source)
        : base(in source, true)
    {
        StrValue = value;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitStringLiteralExpression(this, state, out result);
    }
}
