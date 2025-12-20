using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal class NullCheckerExpression : Expression
{
    internal IdentifierExpression Operand { get; private set; }

    internal NullCheckerExpression(IdentifierExpression operand, SourceInfo source)
        : base(source)
    {
        Operand = operand;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitNullCheckerExpression(this, state);
    }
}
