using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal sealed class BinaryExpression : Expression
{
    internal readonly Expression Left;
    internal readonly string Operator;
    internal readonly Expression Right;

    internal BinaryExpression(Expression left, string op, Expression right, ref readonly SourceInfo source)
        : base(in source, true)
    {
        Left = left;
        Operator = op;
        Right = right;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitBinaryExpression(this, state, out result);
    }
}
