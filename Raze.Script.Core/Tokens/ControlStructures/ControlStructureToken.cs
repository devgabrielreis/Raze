using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.ControlStructures;

internal abstract class ControlStructureToken : Token
{
    public ControlStructureToken(string lexeme, SourceInfo source)
    : base(lexeme, source)
    {
    }
}
