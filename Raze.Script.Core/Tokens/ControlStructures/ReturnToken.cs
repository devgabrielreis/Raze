using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.ControlStructures;

internal class ReturnToken : ControlStructureToken
{
    public ReturnToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
