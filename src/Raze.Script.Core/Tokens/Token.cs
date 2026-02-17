using Raze.Script.Core.Metadata;
using System.Runtime.InteropServices;

namespace Raze.Script.Core.Tokens;

[StructLayout(LayoutKind.Auto)]
internal readonly struct Token
{
    public readonly TokenType Type;
    public readonly string Lexeme;
    public readonly SourceInfo SourceInfo;

    public Token(TokenType type, string lexeme, SourceInfo source)
    {
        Type = type;
        Lexeme = lexeme;
        SourceInfo = source;
    }
}
