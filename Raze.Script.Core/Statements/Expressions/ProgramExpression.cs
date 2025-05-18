namespace Raze.Script.Core.Statements.Expressions;

internal class ProgramExpression : Expression
{
    public IList<Statement> Body { get; set; }

    public ProgramExpression(int startLine = 0, int startColumn = 0)
        : base(startLine, startColumn)
    {
        Body = [];
    }
}
