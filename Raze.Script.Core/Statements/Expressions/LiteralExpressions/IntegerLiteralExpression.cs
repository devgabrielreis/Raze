namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal class IntegerLiteralExpression : LiteralExpression
{
    public override object Value => _value;

    public Int128 IntValue => _value;

    private readonly Int128 _value;

    public IntegerLiteralExpression(Int128 value, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        _value = value;
    }
}
