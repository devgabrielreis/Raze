namespace Raze.Script.Core.Tokens.Primitives;

internal abstract class PrimitiveTypeToken : Token
{
    public PrimitiveTypeToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
