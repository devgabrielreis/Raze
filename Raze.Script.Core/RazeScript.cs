using System.Reflection;
using Raze.Script.Core.AST;
using Raze.Script.Core.Types;

namespace Raze.Script.Core;

public static class RazeScript
{
    public static Version Version => Assembly.GetExecutingAssembly()
                                             .GetName()
                                             .Version!;

    public static RuntimeType Evaluate(string source)
    {
        var tokens = new Lexer.Lexer(source).Tokenize();

        var program = new Parser(tokens).Parse();

        var result = Interpreter.Interpreter.Evaluate(program);

        return result;
    }
}
