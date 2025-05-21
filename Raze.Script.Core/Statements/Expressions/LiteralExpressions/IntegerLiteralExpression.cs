namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal class IntegerLiteralExpression : LiteralExpression
{
    public override object Value => _value;

    private readonly int _value;

    public IntegerLiteralExpression(int value, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        _value = value;
    }
}
