namespace Raze.Script.Core.Tokens.Primitives;

internal class StringPrimitive : PrimitiveTypeToken
{
    public StringPrimitive(string lexeme, int line, int column)
        : base(lexeme, line, column)
    {
    }
}
