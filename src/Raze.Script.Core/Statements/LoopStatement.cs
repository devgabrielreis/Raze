using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal sealed class LoopStatement : Statement
{
    internal readonly IReadOnlyList<Statement> Initialization;

    internal readonly Expression? Condition;
    
    internal readonly Statement? Update;

    internal readonly CodeBlockStatement Body;

    internal LoopStatement(
        List<Statement> initialization,
        Expression? condition,
        Statement? update,
        CodeBlockStatement body,
        ref readonly SourceInfo source
    )
        : base(in source, false)
    {
        Initialization = initialization;
        Condition = condition;
        Update = update;
        Body = body;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitLoopStatement(this, state, out result);
    }
}
