namespace Raze.Script.Core.Tokens.Operators.MultiplicativeOperators;

internal abstract class MultiplicativeOperatorToken : OperatorToken
{
    public MultiplicativeOperatorToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
