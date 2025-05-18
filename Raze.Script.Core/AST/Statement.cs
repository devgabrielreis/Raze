namespace Raze.Script.Core.AST;

public abstract class Statement
{
    public int StartLine { get; private set; }
    public int StartColumn { get; private set; }

    public Statement(int startLine, int startColumn)
    {
        StartLine = startLine;
        StartColumn = startColumn;
    }
}
