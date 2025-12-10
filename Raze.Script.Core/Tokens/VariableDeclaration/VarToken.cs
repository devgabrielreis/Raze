using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.VariableDeclaration;

internal class VarToken : VariableDeclarationToken
{
    public VarToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
