using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal sealed class IfElseStatement : Statement
{
    internal Expression Condition { get; }
    internal CodeBlockStatement Then { get; }
    internal Statement? Else { get; }

    internal IfElseStatement(Expression condition, CodeBlockStatement then, Statement? elseStmt, SourceInfo source)
        : base(source, false)
    {
        Condition = condition;
        Then = then;
        Else = elseStmt;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitIfElseStatement(this, state);
    }
}
