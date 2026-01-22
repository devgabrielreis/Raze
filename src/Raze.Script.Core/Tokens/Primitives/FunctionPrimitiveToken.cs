using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Primitives;

internal class FunctionPrimitiveToken : PrimitiveTypeToken
{
    public FunctionPrimitiveToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
