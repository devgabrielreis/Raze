namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal class FloatLiteralExpression : LiteralExpression
{
    public override object Value => _value;

    private readonly float _value;

    public FloatLiteralExpression(float value, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        _value = value;
    }
}
