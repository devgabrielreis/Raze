using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal sealed class ImportFileStatement : Statement
{
    internal readonly string FilePath;

    internal ImportFileStatement(string filePath, ref readonly SourceInfo source)
        : base(in source, true)
    {
        FilePath = filePath;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitImportFileStatement(this, state, out result);
    }
}
