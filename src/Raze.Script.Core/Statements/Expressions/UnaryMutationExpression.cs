using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal sealed class UnaryMutationExpression : Expression
{
    internal readonly IdentifierExpression Operand;

    internal readonly string Operator;

    internal readonly bool IsPostfix;

    internal UnaryMutationExpression(
        IdentifierExpression operand,
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
        visitor.VisitUnaryMutationExpression(this, state, out result);
    }
}
