namespace Raze.Script.Core.Statements.Expressions;

internal abstract class PrimaryExpression : Expression
{
    public PrimaryExpression(int startLine, int startColumn)
        : base(startLine, startColumn)
    {
    }
}
