using Raze.Script.Core.Exceptions.RuntimeExceptions;

namespace Raze.Tests.Core.ControlStructures;

public class ForLoopTests
{
    [Fact]
    public void Evaluate_ForLoop_ShouldExecuteUntilConditionIsFalse()
    {
        var script = @"
            var integer test = 0;
            for (var integer i = 0; i < 10; i = i + 1) {
                test = test + i;
            }
            test
        ";

        RazeAssert.EvaluatesToInteger(script, 45);
    }

    [Fact]
    public void Evaluate_NonBooleanForLoopCondition_ThrowsUnexpectedTypeException()
    {
        var script = @"
            var integer test = 0;
            for (var integer i = 0; i + 10; i = i + 1) {
                test = test + i;
            }
        ";

        RazeAssert.ReturnsError<UnexpectedTypeException>(script);
    }

    [Fact]
    public void Evaluate_NullForLoopCondition_ThrowsUnexpectedTypeException()
    {
        var script = @"
            var boolean? test = null;
            for (var integer i = 0; test; i = i + 1) {
                test = false;
            }
        ";

        RazeAssert.ReturnsError<UnexpectedTypeException>(script);
    }

    [Fact]
    public void Evaluate_BreakStatement_ExitsForLoop()
    {
        var script = @"
            var integer? test = null;
            for (var integer i = 0; i < 10; i = i + 1) {
                if (i == 5) {
                    break;
                }
                test = i;
            }
            test
        ";

        RazeAssert.EvaluatesToInteger(script, 4);
    }

    [Fact]
    public void Evaluate_ContinueStatement_SkipsForLoopIteration()
    {
        var script = @"
            var integer test = 0;
            for (var integer i = 0; i < 6; i = i + 1) {
                if (i % 2 == 0) {
                    continue;
                }
                test = test + i;
            }
            test
        ";

        RazeAssert.EvaluatesToInteger(script, 9);
    }

    [Fact]
    public void Evaluate_ForLoopParameters_CanBeEmpty()
    {
        var script = @"
            var integer test = 0;
            for (;;) {
                test = test + 1;
                if (test == 10) {
                    break;
                }
            }
            test
        ";

        RazeAssert.EvaluatesToInteger(script, 10);
    }

    [Fact]
    public void Evaluate_ForLoopInitialization_CanUseVariableAlreadyDeclared()
    {
        var script = @"
            var integer test = 500;

            for (test = 0; test < 5; test = test + 1) { }

            test
        ";

        RazeAssert.EvaluatesToInteger(script, 5);
    }

    [Fact]
    public void Evaluate_ForLoopBody_CanHaveConstantDeclaration()
    {
        var script = @"
            var integer test = 0;
            for (var integer i = 0; i < 2; i = i + 1) {
                const integer valueToAdd = 5;
                test = test + valueToAdd;
            }
            test
        ";

        RazeAssert.EvaluatesToInteger(script, 10);
    }
}
