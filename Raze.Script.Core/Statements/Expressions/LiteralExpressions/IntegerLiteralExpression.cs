using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal class IntegerLiteralExpression : LiteralExpression
{
    public override object Value => _value;

    public Int128 IntValue => _value;

    private readonly Int128 _value;

    public IntegerLiteralExpression(Int128 value, SourceInfo source)
        : base(source)
    {
        _value = value;
    }
}
