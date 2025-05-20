namespace Raze.Script.Core.Tokens.VariableDeclaration;

internal abstract class VariableDeclarationToken : Token
{
    public VariableDeclarationToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
