namespace Raze.Script.Core.Statements.Expressions;

internal abstract class Expression : Statement
{
    public Expression(int startLine, int startColumn)
        : base(startLine, startColumn)
    {
    }
}
