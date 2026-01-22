using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal class BinaryExpression : Expression
{
    public Expression Left { get; private set; }
    public string Operator { get; private set; }
    public Expression Right { get; private set; }

    public BinaryExpression(Expression left, string op, Expression right, SourceInfo source)
        : base(source)
    {
        Left = left;
        Operator = op;
        Right = right;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitBinaryExpression(this, state);
    }
}
