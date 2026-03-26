using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal sealed class CodeBlockStatement : Statement
{
    internal IReadOnlyList<Statement> Body { get; private set; }

    internal CodeBlockStatement(IReadOnlyList<Statement> body, SourceInfo source)
        : base(source, false)
    {
        Body = body;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitCodeBlockStatement(this, state);
    }
}
