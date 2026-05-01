using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal sealed class IdentifierExpression : Expression
{
    internal readonly string Symbol;

    internal IdentifierExpression(string symbol, ref readonly SourceInfo source)
        : base(in source, true)
    {
        Symbol = symbol;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitIdentifierExpression(this, state, out result);
    }
}
