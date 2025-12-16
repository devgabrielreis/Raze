using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal class StringLiteralExpression : LiteralExpression
{
    public override object Value => _value;

    public string StrValue => _value;

    private readonly string _value;

    public StringLiteralExpression(string value, SourceInfo source)
        : base(source)
    {
        _value = value;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitStringLiteralExpression(this, state);
    }
}
