using Raze.Script.Core.Engine;
using Raze.Script.Core.Types;
using System.Reflection;

namespace Raze.Script.Core;

public static class RazeScript
{
    public static Version Version => Assembly.GetExecutingAssembly()
                                             .GetName()
                                             .Version!;

    public static RuntimeType Evaluate(string source)
    {
        var tokens = new Lexer(source).Tokenize();

        var program = new Parser(tokens).Parse();

        var result = Interpreter.Evaluate(program);

        return result;
    }
}
