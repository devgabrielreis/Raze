namespace Raze.Script.Core.Tokens.Primitives;

internal class StringPrimitiveToken : PrimitiveTypeToken
{
    public StringPrimitiveToken(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
