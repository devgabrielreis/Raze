using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.VariableDeclaration;

internal class ConstToken : VariableDeclarationToken
{
    public ConstToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
