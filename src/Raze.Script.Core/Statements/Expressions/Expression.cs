using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal abstract class Expression : Statement
{
    internal Expression(SourceInfo source, bool requireSemicolon)
        : base(source, requireSemicolon)
    {
    }
}
