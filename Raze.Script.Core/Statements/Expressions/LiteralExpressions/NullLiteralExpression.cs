using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal class NullLiteralExpression : LiteralExpression
{
    public override object? Value => null;
    public NullLiteralExpression(SourceInfo source)
        : base(source)
    {
    }
}
