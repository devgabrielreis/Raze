namespace Raze.Script.Core.Tokens;

internal class FunctionDeclaration : Token
{
    public FunctionDeclaration(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
