using Raze.Script.Core.Engine;
using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Result;
using Raze.Script.Core.Runtime.Scopes;
using System.Reflection;

namespace Raze.Script.Core;

public static class RazeScript
{
    public static Version Version => Assembly.GetExecutingAssembly()
                                             .GetName()
                                             .Version!;

    public static Scope CreateInterpreterScope()
    {
        return Scope.CreateInterpreterScope();
    }

    public static RazeResult Evaluate(string source, string sourceLocation, Scope? scope = null)
    {
        if (scope is null)
        {
            scope = Scope.CreateInterpreterScope();
        }

        try
        {
            var tokens = Lexer.Tokenize(source, sourceLocation);
            var program = Parser.Parse(tokens, sourceLocation);
            var result = new Interpreter().Evaluate(program, scope);

            return new RazeSuccess(result);
        }
        catch (RazeException razeEx)
        {
            return new RazeError(razeEx);
        }
    }
}
