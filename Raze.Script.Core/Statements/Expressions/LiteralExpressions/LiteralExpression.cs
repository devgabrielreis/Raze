namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal abstract class LiteralExpression : Expression
{
    public abstract object? Value { get; }

    public LiteralExpression(int startLine, int startColumn)
        : base(startLine, startColumn)
    {
    }
}
