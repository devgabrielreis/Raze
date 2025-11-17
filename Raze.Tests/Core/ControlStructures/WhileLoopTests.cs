using Raze.Script.Core;
using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Values;

namespace Raze.Tests.Core.ControlStructures;

public class WhileLoopTests
{
    [Fact]
    public void Evaluate_WhileLoop_ShouldExecuteUntilConditionIsFalse()
    {
        var script = @"
            var integer test = 0;
            while (test < 10) {
                test = test + 3;
            }
            test
        ";

        var result = RazeScript.Evaluate(script);
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(12, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_NonBooleanWhileLoopCondition_ThrowsUnexpectedTypeException()
    {
        var script = @"
            var integer test = 0;
            while (test + 20) {
                test = test + i;
                break;
            }
        ";

        Assert.Throws<UnexpectedTypeException>(() =>
        {
            RazeScript.Evaluate(script);
        });
    }

    [Fact]
    public void Evaluate_NullWhileLoopCondition_ThrowsUnexpectedTypeException()
    {
        var script = @"
            var boolean? test = null;
            while (test) {
                test = false;
            }
        ";

        Assert.Throws<UnexpectedTypeException>(() =>
        {
            RazeScript.Evaluate(script);
        });
    }

    [Fact]
    public void Evaluate_BreakStatement_ExitsWhileLoop()
    {
        var script = @"
            var integer? test = 0;
            while (true) {
                if (test == 5) {
                    break;
                }
                test = test + 1;
            }
            test
        ";

        var result = RazeScript.Evaluate(script);
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(5, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_ContinueStatement_SkipsWhileLoopIteration()
    {
        var script = @"
            var integer? test = 0;
            while (test < 10) {
                test = test + 3;
                continue;
                
                test = 1000;
            }
            test
        ";

        var result = RazeScript.Evaluate(script);
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(12, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_EmptyWhileLoopCondition_ThrowsUnexpectedTokenException()
    {
        var script = @"
            while () {
                break;
            }
        ";

        Assert.Throws<UnexpectedTokenException>(() =>
        {
            RazeScript.Evaluate(script);
        });
    }

    [Fact]
    public void Evaluate_WhileLoopBody_CanHaveConstantDeclaration()
    {
        var script = @"
            var integer test = 0;
            while (test < 10) {
                const integer valueToAdd = 5;
                test = test + valueToAdd;
            }
            test
        ";
        Int128 expectedResult = 10;

        var result = RazeScript.Evaluate(script);

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(expectedResult, (result as IntegerValue)!.IntValue);
    }
}
