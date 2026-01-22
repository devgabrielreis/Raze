using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.VariableDeclaration;

internal abstract class VariableDeclarationToken : Token
{
    public VariableDeclarationToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
