namespace Raze.Script.Core.AST;

internal class IdentifierExpression : Expression
{
    public string Symbol { get; set; }

    public IdentifierExpression(string symbol)
    {
        Symbol = symbol;
    }
}
