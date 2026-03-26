using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal sealed class BinaryExpression : Expression
{
    internal Expression Left { get; private set; }
    internal string Operator { get; private set; }
    internal Expression Right { get; private set; }

    internal BinaryExpression(Expression left, string op, Expression right, SourceInfo source)
        : base(source, true)
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
