namespace Raze.Script.Core.Tokens.Operators;

internal class AssignmentToken : OperatorToken
{
    public AssignmentToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
