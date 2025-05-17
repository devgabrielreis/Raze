namespace Raze.Script.Core.AST;

internal class ProgramStatement : Statement
{
    public IList<Statement> Body { get; set; }

    public ProgramStatement()
    {
        Body = [];
    }
}
