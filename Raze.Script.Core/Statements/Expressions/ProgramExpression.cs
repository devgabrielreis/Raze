namespace Raze.Script.Core.Statements.Expressions;

internal class ProgramExpression : Expression
{
    public IList<Statement> Body { get; set; }
    public override bool RequireSemicolon => false;

    public ProgramExpression(int startLine = 0, int startColumn = 0)
        : base(startLine, startColumn)
    {
        Body = [];
    }
}
