using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.ControlStructures;

internal class BreakToken : ControlStructureToken
{
    public BreakToken(string lexeme, SourceInfo source)
    : base(lexeme, source)
    {
    }
}
