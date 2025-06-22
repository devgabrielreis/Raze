namespace Raze.Script.Core.Statements;

internal class CodeBlockStatement : Statement
{
    public IList<Statement> Body { get; set; }
    public override bool RequireSemicolon => false;

    public CodeBlockStatement(int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        Body = [];
    }
}
