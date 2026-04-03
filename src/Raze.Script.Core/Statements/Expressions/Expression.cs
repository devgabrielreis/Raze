using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal abstract class Expression : Statement
{
    internal Expression(ref readonly SourceInfo source, bool requireSemicolon)
        : base(in source, requireSemicolon)
    {
    }
}
