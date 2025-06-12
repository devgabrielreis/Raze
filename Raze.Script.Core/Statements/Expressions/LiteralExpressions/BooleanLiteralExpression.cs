namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal class BooleanLiteralExpression : LiteralExpression
{
    public override object Value => _value;

    public bool BoolValue => _value;

    private readonly bool _value;

    public BooleanLiteralExpression(bool value, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        _value = value;
    }
}
