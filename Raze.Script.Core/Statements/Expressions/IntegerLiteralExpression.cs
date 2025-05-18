namespace Raze.Script.Core.Statements.Expressions;

internal class IntegerLiteralExpression : PrimaryExpression
{
    public int Value { get; private set; }

    public IntegerLiteralExpression(int value, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        Value = value;
    }
}
