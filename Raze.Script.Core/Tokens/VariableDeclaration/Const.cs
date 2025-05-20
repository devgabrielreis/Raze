namespace Raze.Script.Core.Tokens.VariableDeclaration;

internal class Const : VariableDeclarationToken
{
    public Const(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
