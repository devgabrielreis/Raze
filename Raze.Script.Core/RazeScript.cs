using Raze.Script.Core.Lexer;

namespace Raze.Script.Core;

public class RazeScript
{
    public IEnumerable<Token> Tokenize(string source)
    {
        return new Lexer.Lexer(source).Tokenize();
    }
}
