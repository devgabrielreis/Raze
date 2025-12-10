using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.ControlStructures;

internal class ContinueToken : ControlStructureToken
{
    public ContinueToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
