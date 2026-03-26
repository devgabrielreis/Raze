using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions.LiteralExpressions;

internal abstract class LiteralExpression : Expression
{
    internal LiteralExpression(SourceInfo source, bool requireSemicolon)
        : base(source, requireSemicolon)
    {
    }
}
