using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal sealed class LoopStatement : Statement
{
    internal IReadOnlyList<Statement> Initialization { get; }

    internal Expression? Condition { get; }
    
    internal Statement? Update { get; }

    internal CodeBlockStatement Body { get; }

    internal LoopStatement(List<Statement> initialization, Expression? condition, Statement? update, CodeBlockStatement body, SourceInfo source)
        : base(source, false)
    {
        Initialization = initialization;
        Condition = condition;
        Update = update;
        Body = body;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitLoopStatement(this, state);
    }
}
