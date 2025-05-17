namespace Raze.Script.Core.AST;

internal class IntegerLiteralExpression : Expression
{
    public int Value { get; set; }

    public IntegerLiteralExpression(int value)
    {
        Value = value;
    }
}
