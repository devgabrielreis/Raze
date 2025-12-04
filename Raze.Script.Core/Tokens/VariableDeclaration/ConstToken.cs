namespace Raze.Script.Core.Tokens.VariableDeclaration;

internal class ConstToken : VariableDeclarationToken
{
    public ConstToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
