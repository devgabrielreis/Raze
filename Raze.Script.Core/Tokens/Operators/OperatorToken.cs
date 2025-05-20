namespace Raze.Script.Core.Tokens.Operators;

internal abstract class OperatorToken : Token
{
    public OperatorToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
