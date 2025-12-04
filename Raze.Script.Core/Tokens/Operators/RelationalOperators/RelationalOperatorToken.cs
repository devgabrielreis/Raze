namespace Raze.Script.Core.Tokens.Operators.RelationalOperators;

internal abstract class RelationalOperatorToken : OperatorToken
{
    public RelationalOperatorToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
