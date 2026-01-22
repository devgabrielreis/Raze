using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal class DecimalLiteralExpression : LiteralExpression
{
    public override object Value => _value;

    public decimal DecValue => _value;

    private readonly decimal _value;

    public DecimalLiteralExpression(decimal value, SourceInfo source)
        : base(source)
    {
        _value = value;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitDecimalLiteralExpression(this, state);
    }
}
