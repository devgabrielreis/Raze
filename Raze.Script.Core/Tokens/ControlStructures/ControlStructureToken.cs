namespace Raze.Script.Core.Tokens.ControlStructures;

internal abstract class ControlStructureToken : Token
{
    public ControlStructureToken(string lexeme, int line, int column)
    : base(lexeme, line, column)
    {
    }
}
