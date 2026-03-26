using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal abstract class Statement
{
    internal SourceInfo SourceInfo { get; private set; }

    internal readonly bool RequireSemicolon;

    internal Statement(SourceInfo source, bool requireSemicolon)
    {
        SourceInfo = source;
        RequireSemicolon = requireSemicolon;
    }

    internal abstract TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state);
}
