namespace Raze.Script.Core.Tokens.Operators.RelationalOperators;

internal abstract class RelationalOperator : OperatorToken
{
    public RelationalOperator(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
