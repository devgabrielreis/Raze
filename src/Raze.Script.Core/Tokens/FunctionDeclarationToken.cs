using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens;

internal class FunctionDeclarationToken : Token
{
    public FunctionDeclarationToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
