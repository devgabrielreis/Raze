using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators.MultiplicativeOperators;

internal class ModuloToken : MultiplicativeOperatorToken
{
    public ModuloToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
