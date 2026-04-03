using Raze.Script.Core.Metadata;
using System.Runtime.InteropServices;

namespace Raze.Script.Core.Tokens;

[StructLayout(LayoutKind.Auto)]
internal readonly struct Token
{
    internal readonly TokenType Type;
    internal readonly string Lexeme;
    internal readonly SourceInfo SourceInfo;

    internal Token(TokenType type, string lexeme, ref readonly SourceInfo source)
    {
        Type = type;
        Lexeme = lexeme;
        SourceInfo = source;
    }
}
