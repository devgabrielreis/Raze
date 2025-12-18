using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Tokens.Operators;

namespace Raze.Script.Core.Statements.Expressions;

internal class BinaryExpression : Expression
{
    public Expression Left { get; private set; }
    public OperatorToken Operator { get; private set; }
    public Expression Right { get; private set; }

    public BinaryExpression(Expression left, OperatorToken op, Expression right, SourceInfo source)
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
