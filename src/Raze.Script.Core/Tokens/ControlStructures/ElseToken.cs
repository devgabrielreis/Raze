using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.ControlStructures;

internal class ElseToken : ControlStructureToken
{
    public ElseToken(string lexeme, SourceInfo source)
    : base(lexeme, source)
    {
    }
}
