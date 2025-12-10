using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal class IdentifierExpression : Expression
{
    public string Symbol { get; private set; }

    public IdentifierExpression(string symbol, SourceInfo source)
        : base(source)
    {
        Symbol = symbol;
    }
}
