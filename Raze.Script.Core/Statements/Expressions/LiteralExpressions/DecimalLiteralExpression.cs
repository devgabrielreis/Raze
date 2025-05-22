namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal class DecimalLiteralExpression : LiteralExpression
{
    public override object Value => _value;

    private readonly decimal _value;

    public DecimalLiteralExpression(decimal value, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        _value = value;
    }
}
