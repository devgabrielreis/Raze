using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;

namespace Raze.Tests.Core;

public class ScopeTests
{
    [Fact]
    public void Evaluate_DuplicateVariableDeclaration_ThrowsException()
    {
        var scope = RazeScript.CreateInterpreterScope();

        RazeAssert.EvaluatesToVoid("var integer x = 1", scope);
        RazeAssert.ReturnsError<RedeclarationException>("var integer x = 2", scope);

        var scope2 = RazeScript.CreateInterpreterScope();

        RazeAssert.EvaluatesToVoid("var integer x = 1", scope2);
        RazeAssert.ReturnsError<RedeclarationException>("const integer x = 2", scope2);
    }

    [Fact]
    public void Evaluate_CodeBlock_CanAcessOutsideScope()
    {
        var script = @"
            var integer a = 10;
            {
                a = a + 1;
                {
                    a = a + 2;
                    {
                        a = a + 3;
                    }
                }
            }
            a;
        ";

        RazeAssert.EvaluatesToInteger(script, 16);
    }

    [Fact]
    public void Evaluate_VariableDeclaredInsideCodeBlock_ShouldntExistOutsideOfIt()
    {
        var script = @"
            {
                var integer test = 10;
            }
            test
        ";

        RazeAssert.ReturnsError<UndefinedIdentifierException>(script);
    }

    [Fact]
    public void Evaluate_VariableDeclaredInsideForLoopConditionOutsideOfIt_ThrowsUndefinedIdentifierException()
    {
        var scope = RazeScript.CreateInterpreterScope();

        var script = @"
            for (var integer i = 0; i < 10; i = i + 1) {
                var integer test = i;
            }
        ";

        RazeAssert.EvaluatesToVoid(script, scope);
        RazeAssert.ReturnsError<UndefinedIdentifierException>("test", scope);
        RazeAssert.ReturnsError<UndefinedIdentifierException>("i", scope);
    }

    [Fact]
    public void Evaluate_VariableDeclaredInsideWhileLoopConditionOutsideOfIt_ThrowsUndefinedIdentifierException()
    {
        var scope = RazeScript.CreateInterpreterScope();

        var script = @"
            while (true) {
                var integer test = 10;
                break;
            }
        ";

        RazeAssert.EvaluatesToVoid(script, scope);
        RazeAssert.ReturnsError<UndefinedIdentifierException>("test", scope);
    }

    [Fact]
    public void Evaluate_FunctionTryingToAccessCallerScope_ThrowsUndefinedIdentifierException()
    {
        var script = @"
            def void outer() {
	            var integer cantAccess = 5;
	            inner();
            }

            def void inner() {
	            cantAccess = 6;
            }

            outer();
        ";

        RazeAssert.ReturnsError<UndefinedIdentifierException>(script);
    }
}
