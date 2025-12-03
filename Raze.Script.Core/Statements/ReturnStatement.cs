using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal class ReturnStatement : Statement
{
    public Expression? ReturnedValue { get; private set; }

    public ReturnStatement(Expression? returnedValue, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        ReturnedValue = returnedValue;
    }
}
