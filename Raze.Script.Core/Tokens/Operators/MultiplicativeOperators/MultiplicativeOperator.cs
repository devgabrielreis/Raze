namespace Raze.Script.Core.Tokens.Operators.MultiplicativeOperators;

internal abstract class MultiplicativeOperator : OperatorToken
{
    public MultiplicativeOperator(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
