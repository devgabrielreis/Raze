using Raze.Script.Core;
using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Values;

namespace Raze.Tests;

public class RazeScriptControlStructuresTests
{
    [Fact]
    public void Evaluate_IfWithTrueCondition_ShouldExecute()
    {
        var script = @"
            var integer test = 0;
            if (10 > 9) {
                test = 10;
            }
            test
        ";

        var result = RazeScript.Evaluate(script);
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(10, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_IfWithFalseCondition_ShouldntExecute()
    {
        var script = @"
            var integer test = 0;
            if (9 > 9) {
                test = 10;
            }
            test
        ";

        var result = RazeScript.Evaluate(script);
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(0, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_IfElseWithFalseCondition_ShouldExecuteElse()
    {
        var script = @"
            var integer test = 0;
            if (9 > 9) {
                test = 10;
            } else {
                test = 20;
            }
            test
        ";

        var result = RazeScript.Evaluate(script);
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(20, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_IfElse_CanBeChained()
    {
        var script = @"
            var integer test = 0;
            if (9 > 9) {
                test = 10;
            } else if (9 > 9) {
                test = 20;
            } else if (9 > 9) {
                test = 30;
            } else if (9 > 9) {
                test = 40;
            } else if (9 > 0) {
                test = 50;
            }
            test
        ";

        var result = RazeScript.Evaluate(script);
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(50, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_EmptyIfCondition_ThrowsUnexpectedTokenException()
    {
        var script = @"
            var integer test = 0;
            if () {
                test = 10;
            }
            test
        ";

        Assert.Throws<UnexpectedTokenException>(() =>
        {
            RazeScript.Evaluate(script);
        });
    }

    [Fact]
    public void Evaluate_NonBooleanIfCondition_ThrowsUnexpectedTypeException()
    {
        var script = @"
            var integer test = 0;
            if (10 + 10) {
                test = 10;
            }
            test
        ";

        Assert.Throws<UnexpectedTypeException>(() =>
        {
            RazeScript.Evaluate(script);
        });
    }

    [Fact]
    public void Evaluate_NullBooleanIfCondition_ThrowsNullValueException()
    {
        var script = @"
            var integer test = 0;
            var boolean nullBoolean;
            if (nullBoolean) {
                test = 10;
            }
            test
        ";

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate(script);
        });
    }
}
