using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal class BooleanLiteralExpression : LiteralExpression
{
    public override object Value => _value;

    public bool BoolValue => _value;

    private readonly bool _value;

    public BooleanLiteralExpression(bool value, SourceInfo source)
        : base(source)
    {
        _value = value;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitBooleanLiteralExpression(this, state);
    }
}
