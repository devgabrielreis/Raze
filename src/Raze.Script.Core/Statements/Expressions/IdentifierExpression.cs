using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal sealed class IdentifierExpression : Expression
{
    internal string Symbol { get; private set; }

    internal IdentifierExpression(string symbol, SourceInfo source)
        : base(source, true)
    {
        Symbol = symbol;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitIdentifierExpression(this, state);
    }
}
