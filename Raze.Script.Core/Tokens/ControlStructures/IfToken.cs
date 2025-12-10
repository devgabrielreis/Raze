using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.ControlStructures;

internal class IfToken : ControlStructureToken
{
    public IfToken(string lexeme, SourceInfo source)
    : base(lexeme, source)
    {
    }
}
