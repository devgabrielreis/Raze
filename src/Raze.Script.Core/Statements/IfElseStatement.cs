using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal sealed class IfElseStatement : Statement
{
    internal readonly Expression Condition;
    internal readonly CodeBlockStatement Then;
    internal readonly Statement? Else;

    internal IfElseStatement(
        Expression condition,
        CodeBlockStatement then,
        Statement? elseStmt,
        ref readonly SourceInfo source
    )
        : base(in source, false)
    {
        Condition = condition;
        Then = then;
        Else = elseStmt;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitIfElseStatement(this, state, out result);
    }
}
