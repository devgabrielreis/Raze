namespace Raze.Script.Core.AST;

public abstract class Expression : Statement
{
    public Expression(int startLine, int startColumn)
        : base(startLine, startColumn)
    {
    }
}
