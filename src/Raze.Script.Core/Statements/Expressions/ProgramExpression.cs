using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal sealed class ProgramExpression : Expression
{
    internal readonly IReadOnlyList<Statement> Body;

    internal ProgramExpression(IReadOnlyList<Statement> body, ref readonly SourceInfo source)
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
        visitor.VisitProgramExpression(this, state, out result);
    }
}
