namespace Raze.Script.Core.AST;

internal class IdentifierExpression : PrimaryExpression
{
    public string Symbol { get; set; }

    public IdentifierExpression(string symbol)
    {
        Symbol = symbol;
    }
}
