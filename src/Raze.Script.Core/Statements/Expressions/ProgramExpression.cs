using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal class ProgramExpression : Expression
{
    public IReadOnlyList<Statement> Body { get; private set; }

    public override bool RequireSemicolon => false;

    public ProgramExpression(IReadOnlyList<Statement> body, SourceInfo source)
        : base(source)
    {
        Body = body;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitProgramExpression(this, state);
    }
}
