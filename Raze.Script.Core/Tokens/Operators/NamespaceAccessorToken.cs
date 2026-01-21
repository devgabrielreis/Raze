using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Tokens.Operators;

internal class NamespaceAccessorToken : OperatorToken
{
    internal NamespaceAccessorToken(string lexeme, SourceInfo source)
        : base(lexeme, source)
    {
    }
}
