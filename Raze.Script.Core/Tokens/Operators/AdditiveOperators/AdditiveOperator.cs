namespace Raze.Script.Core.Tokens.Operators.AdditiveOperators;

internal abstract class AdditiveOperator : OperatorToken
{
    public AdditiveOperator(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
