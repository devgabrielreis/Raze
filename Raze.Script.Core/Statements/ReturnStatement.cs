using Raze.Script.Core.Metadata;
using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal class ReturnStatement : Statement
{
    public Expression? ReturnedValue { get; private set; }

    public ReturnStatement(Expression? returnedValue, SourceInfo source)
        : base(source)
    {
        ReturnedValue = returnedValue;
    }
}
