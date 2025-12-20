using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Tokens.Operators;

namespace Raze.Script.Core.Statements.Expressions;

internal class UnaryMutationExpression : Expression
{
    internal IdentifierExpression Operand { get; private set; }

    internal OperatorToken Operator { get; private set; }

    internal bool IsPostfix { get; private set; }

    internal UnaryMutationExpression(IdentifierExpression operand, OperatorToken op, bool isPostfix, SourceInfo source)
        : base(source)
    {
        Operand = operand;
        Operator = op;
        IsPostfix = isPostfix;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitUnaryMutationExpression(this, state);
    }
}
