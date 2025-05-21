namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal class NullLiteralExpression : LiteralExpression
{
    public override object? Value => null;
    public NullLiteralExpression(int startLine, int startColumn)
        : base(startLine, startColumn)
    {
    }
}
