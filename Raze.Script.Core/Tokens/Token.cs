using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens;

internal abstract class Token
{
    public string Lexeme { get; private set; }

    public SourceInfo SourceInfo { get; private set; }

    public Token(string lexeme, SourceInfo source)
    {
        Lexeme = lexeme;
        SourceInfo = source;
    }
}
