using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal sealed class ProgramExpression : Expression
{
    internal IReadOnlyList<Statement> Body { get; private set; }

    internal ProgramExpression(IReadOnlyList<Statement> body, SourceInfo source)
        : base(source, false)
    {
        Body = body;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitProgramExpression(this, state);
    }
}
