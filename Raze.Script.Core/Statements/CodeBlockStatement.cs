using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal class CodeBlockStatement : Statement
{
    public IList<Statement> Body { get; set; }
    public override bool RequireSemicolon => false;

    public CodeBlockStatement(SourceInfo source)
        : base(source)
    {
        Body = [];
    }
}
