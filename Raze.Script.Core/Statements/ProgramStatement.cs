namespace Raze.Script.Core.Statements;

internal class ProgramStatement : Statement
{
    public IList<Statement> Body { get; set; }

    public ProgramStatement(int startLine = 0, int startColumn = 0)
        : base(startLine, startColumn)
    {
        Body = [];
    }
}
