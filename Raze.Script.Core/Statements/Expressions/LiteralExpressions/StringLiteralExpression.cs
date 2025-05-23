namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal class StringLiteralExpression : LiteralExpression
{
    public override object Value => _value;

    private readonly string _value;

    public StringLiteralExpression(string value, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        _value = value;
    }
}
