namespace Raze.Script.Core.Tokens.Operators.EqualityOperators;

internal abstract class EqualityOperatorToken : OperatorToken
{
    public EqualityOperatorToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
