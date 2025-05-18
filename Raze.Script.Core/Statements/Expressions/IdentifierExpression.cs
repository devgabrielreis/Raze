namespace Raze.Script.Core.Statements.Expressions;

internal class IdentifierExpression : PrimaryExpression
{
    public string Symbol { get; private set; }

    public IdentifierExpression(string symbol, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        Symbol = symbol;
    }
}
