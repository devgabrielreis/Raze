using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.ControlStructures;

internal class WhileToken : ControlStructureToken
{
    public WhileToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
