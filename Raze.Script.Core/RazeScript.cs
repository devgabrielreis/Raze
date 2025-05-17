using Raze.Script.Core.AST;
using Raze.Script.Core.Lexer;

namespace Raze.Script.Core;

public class RazeScript
{
    public IEnumerable<Token> Tokenize(string source)
    {
        var tokens = new Lexer.Lexer(source).Tokenize();

        var program = new Parser(tokens).Parse();

        return tokens;
    }
}
