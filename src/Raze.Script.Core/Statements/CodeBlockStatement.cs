using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal sealed class CodeBlockStatement : Statement
{
    internal readonly IReadOnlyList<Statement> Body;

    internal CodeBlockStatement(IReadOnlyList<Statement> body, ref readonly SourceInfo source)
        : base(in source, false)
    {
        Body = body;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitCodeBlockStatement(this, state, out result);
    }
}
