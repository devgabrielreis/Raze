using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.ControlStructures;

internal class ForToken : ControlStructureToken
{
    public ForToken(string lexeme, SourceInfo source)
    : base(lexeme, source)
    {
    }
}
