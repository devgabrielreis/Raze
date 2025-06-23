using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;

namespace Raze.Tests;

public class RazeScriptScopeTests
{
    [Fact]
    public void Evaluate_DuplicateVariableDeclaration_ThrowsException()
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate("var integer x = 1", scope);

        Assert.Throws<RedeclarationException>(() =>
        {
            RazeScript.Evaluate("var integer x = 2", scope);
        });

        var scope2 = new InterpreterScope();
        RazeScript.Evaluate("var integer x = 1", scope2);

        Assert.Throws<RedeclarationException>(() =>
        {
            RazeScript.Evaluate("const integer x = 2", scope2);
        });
    }

    [Fact]
    public void Evaluate_ConstantAssignment_ThrowsConstantAssignmentException()
    {
        var scope = new InterpreterScope();

        RazeScript.Evaluate("const integer test = 10", scope);

        Assert.Throws<ConstantAssignmentException>(() =>
        {
            RazeScript.Evaluate("test = 11", scope);
        });
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

        var result = RazeScript.Evaluate(script);
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(16, (result as IntegerValue)!.IntValue);
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

        Assert.Throws<UndefinedIdentifierException>(() =>
        {
            RazeScript.Evaluate(script);
        });
    }

    [Fact]
    public void Evaluate_UninitializedVariable_ThrowsUninitializedVariableException()
    {
        var scope = new InterpreterScope();

        RazeScript.Evaluate("var integer variable", scope);
        RazeScript.Evaluate("var integer? nullableVariable", scope);

        Assert.Throws<UninitializedVariableException>(() =>
        {
            RazeScript.Evaluate("variable", scope);
        });

        Assert.Throws<UninitializedVariableException>(() =>
        {
            RazeScript.Evaluate("nullableVariable", scope);
        });
    }
}
