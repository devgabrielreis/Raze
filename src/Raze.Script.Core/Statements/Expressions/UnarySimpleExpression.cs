using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal class UnarySimpleExpression : Expression
{
    internal Expression Operand { get; private set; }

    internal string Operator { get; private set; }

    internal bool IsPostfix { get; private set; }

    internal UnarySimpleExpression(Expression operand, string op, bool isPostfix, SourceInfo source)
        : base(source)
    {
        Operand = operand;
        Operator = op;
        IsPostfix = isPostfix;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitUnarySimpleExpression(this, state);
    }
}
