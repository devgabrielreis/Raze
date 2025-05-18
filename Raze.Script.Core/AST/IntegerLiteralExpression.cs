namespace Raze.Script.Core.AST;

internal class IntegerLiteralExpression : PrimaryExpression
{
    public int Value { get; set; }

    public IntegerLiteralExpression(int value)
    {
        Value = value;
    }
}
