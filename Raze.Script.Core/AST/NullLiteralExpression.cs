namespace Raze.Script.Core.AST;

internal class NullLiteralExpression : PrimaryExpression
{
    public NullLiteralExpression(int startLine, int startColumn)
        : base(startLine, startColumn)
    {
    }
}
