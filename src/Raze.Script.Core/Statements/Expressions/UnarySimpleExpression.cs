using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal sealed class UnarySimpleExpression : Expression
{
    internal readonly Expression Operand;

    internal readonly string Operator;

    internal readonly bool IsPostfix;

    internal UnarySimpleExpression(
        Expression operand,
        string op,
        bool isPostfix,
        ref readonly SourceInfo source
    )
        : base(in source, true)
    {
        Operand = operand;
        Operator = op;
        IsPostfix = isPostfix;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitUnarySimpleExpression(this, state, out result);
    }
}
