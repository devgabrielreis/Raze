using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal sealed class ImportModuleStatement : Statement
{
    internal readonly string ModuleName;

    internal ImportModuleStatement(string moduleName, ref readonly SourceInfo source)
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
        visitor.VisitImportModuleStatement(this, state, out result);
    }
}
