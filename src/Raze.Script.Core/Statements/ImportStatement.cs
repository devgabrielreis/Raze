using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal sealed class ImportStatement : Statement
{
    internal readonly string ModuleName;

    internal ImportStatement(string moduleName, ref readonly SourceInfo source)
        : base(in source, true)
    {
        ModuleName = moduleName;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitImportStatement(this, state, out result);
    }
}
