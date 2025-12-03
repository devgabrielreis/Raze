namespace Raze.Script.Core.Statements.Expressions;

internal class CallExpression : Expression
{
    public Expression Caller { get; private set; }

    public IReadOnlyList<Expression> ArgumentList { get; private set; }

    public CallExpression(Expression caller, IReadOnlyList<Expression> argumentList, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        Caller = caller;
        ArgumentList = argumentList;
    }
}
