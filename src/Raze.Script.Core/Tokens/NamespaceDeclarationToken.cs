using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens;

internal class NamespaceDeclarationToken : Token
{
    internal NamespaceDeclarationToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}