using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal abstract class Statement
{
    internal readonly SourceInfo SourceInfo;

    internal readonly bool RequireSemicolon;

    internal Statement(ref readonly SourceInfo source, bool requireSemicolon)
    {
        SourceInfo = source;
        RequireSemicolon = requireSemicolon;
    }

    internal abstract void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    );
}
