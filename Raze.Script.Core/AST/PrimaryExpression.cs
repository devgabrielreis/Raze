namespace Raze.Script.Core.AST;

internal abstract class PrimaryExpression : Expression
{
    public PrimaryExpression(int startLine, int startColumn)
        : base(startLine, startColumn)
    {
    }
}
