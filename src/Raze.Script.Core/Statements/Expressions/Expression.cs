using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal abstract class Expression : Statement
{
    public Expression(SourceInfo source)
        : base(source)
    {
    }
}
