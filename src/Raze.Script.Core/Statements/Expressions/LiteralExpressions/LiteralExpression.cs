using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal abstract class LiteralExpression : Expression
{
    public abstract object? Value { get; }

    public LiteralExpression(SourceInfo source)
        : base(source)
    {
    }
}
