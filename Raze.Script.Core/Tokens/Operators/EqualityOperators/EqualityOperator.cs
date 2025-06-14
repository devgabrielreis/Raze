namespace Raze.Script.Core.Tokens.Operators.EqualityOperators;

internal abstract class EqualityOperator : OperatorToken
{
    public EqualityOperator(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
