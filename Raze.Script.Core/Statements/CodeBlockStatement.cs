using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal class CodeBlockStatement : Statement
{
    public IReadOnlyList<Statement> Body { get; private set; }

    public override bool RequireSemicolon => false;

    public CodeBlockStatement(IReadOnlyList<Statement> body, SourceInfo source)
        : base(source)
    {
        Body = body;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitCodeBlockStatement(this, state);
    }
}
