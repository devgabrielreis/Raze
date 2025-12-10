using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Primitives;

internal class StringPrimitiveToken : PrimitiveTypeToken
{
    public StringPrimitiveToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
