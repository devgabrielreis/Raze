using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal abstract class Statement
{
    public SourceInfo SourceInfo { get; private set; }

    public virtual bool RequireSemicolon => true;

    public Statement(SourceInfo source)
    {
        SourceInfo = source;
    }
}
