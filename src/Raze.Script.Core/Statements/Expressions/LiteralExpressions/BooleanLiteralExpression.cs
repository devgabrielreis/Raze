using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal sealed class BooleanLiteralExpression : LiteralExpression
{
    internal readonly bool BoolValue;

    internal BooleanLiteralExpression(bool value, SourceInfo source)
        : base(source, true)
    {
        BoolValue = value;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitBooleanLiteralExpression(this, state);
    }
}
