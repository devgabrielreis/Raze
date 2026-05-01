using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal sealed class NullCheckerExpression : Expression
{
    internal readonly Expression Operand;

    internal NullCheckerExpression(Expression operand, ref readonly SourceInfo source)
        : base(in source, true)
    {
        Operand = operand;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitNullCheckerExpression(this, state, out result);
    }
}
