using Raze.Script.Core.Engine;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;
using System.Reflection;

namespace Raze.Script.Core;

public static class RazeScript
{
    public static Version Version => Assembly.GetExecutingAssembly()
                                             .GetName()
                                             .Version!;

    public static RuntimeValue Evaluate(string source, string sourceLocation, Scope? scope = null)
    {
        if (scope is null)
        {
            scope = new InterpreterScope();
        }

        var tokens = new Lexer(source, sourceLocation).Tokenize();

        var program = new Parser(tokens, sourceLocation).Parse();

        var result = new Interpreter().Evaluate(program, scope);

        return result;
    }
}
