using Raze.Script.Core.Engine;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Statements.Expressions;
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

        var program = BuildProgramExpression(source, sourceLocation);

        var result = new Interpreter().Evaluate(program, scope);

        return result;
    }

    private static ProgramExpression BuildProgramExpression(string source, string sourceLocation)
    {
        var tokens = Lexer.Tokenize(source, sourceLocation);
        var program = Parser.Parse(tokens, sourceLocation);

        return program;
    }
}
