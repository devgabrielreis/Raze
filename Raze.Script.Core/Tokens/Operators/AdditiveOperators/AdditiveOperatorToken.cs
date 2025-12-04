namespace Raze.Script.Core.Tokens.Operators.AdditiveOperators;

internal abstract class AdditiveOperatorToken : OperatorToken
{
    public AdditiveOperatorToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
