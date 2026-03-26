using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal sealed class IntegerLiteralExpression : LiteralExpression
{
    internal readonly Int128 IntValue;

    internal IntegerLiteralExpression(Int128 value, SourceInfo source)
        : base(source, true)
    {
        IntValue = value;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitIntegerLiteralExpression(this, state);
    }
}
