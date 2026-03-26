using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal sealed class StringLiteralExpression : LiteralExpression
{
    internal readonly string StrValue;

    internal StringLiteralExpression(string value, SourceInfo source)
        : base(source, true)
    {
        StrValue = value;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitStringLiteralExpression(this, state);
    }
}
