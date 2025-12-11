using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Values;

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

        var result = RazeScript.Evaluate(script, "Raze.Tests");
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(45, (result as IntegerValue)!.IntValue);
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

        Assert.Throws<UnexpectedTypeException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
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

        Assert.Throws<UnexpectedTypeException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
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

        var result = RazeScript.Evaluate(script, "Raze.Tests");
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(4, (result as IntegerValue)!.IntValue);
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

        var result = RazeScript.Evaluate(script, "Raze.Tests");
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(9, (result as IntegerValue)!.IntValue);
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

        var result = RazeScript.Evaluate(script, "Raze.Tests");
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(10, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_ForLoopInitialization_CanUseVariableAlreadyDeclared()
    {
        var script = @"
            var integer test = 500;

            for (test = 0; test < 5; test = test + 1) { }

            test
        ";
        Int128 expectedResult = 5;

        var result = RazeScript.Evaluate(script, "Raze.Tests");

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(expectedResult, (result as IntegerValue)!.IntValue);
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
        Int128 expectedResult = 10;

        var result = RazeScript.Evaluate(script, "Raze.Tests");

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(expectedResult, (result as IntegerValue)!.IntValue);
    }
}
