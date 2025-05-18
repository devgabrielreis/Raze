namespace Raze.Script.Core.Statements.Expressions;

public abstract class Expression : Statement
{
    public Expression(int startLine, int startColumn)
        : base(startLine, startColumn)
    {
    }
}
