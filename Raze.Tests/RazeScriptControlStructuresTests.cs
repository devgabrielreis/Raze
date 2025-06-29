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
            var boolean? nullBoolean = null;
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

        var result = RazeScript.Evaluate(script);
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
            RazeScript.Evaluate(script);
        });
    }

    [Fact]
    public void Evaluate_NullForLoopCondition_ThrowsNullValueException()
    {
        var script = @"
            var boolean? test = null;
            for (var integer i = 0; test; i = i + 1) {
                test = false;
            }
        ";

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate(script);
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

        var result = RazeScript.Evaluate(script);
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

        var result = RazeScript.Evaluate(script);
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

        var result = RazeScript.Evaluate(script);
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(10, (result as IntegerValue)!.IntValue);
    }

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
    public void Evaluate_NullWhileLoopCondition_ThrowsNullValueException()
    {
        var script = @"
            var boolean? test = null;
            while (test) {
                test = false;
            }
        ";

        Assert.Throws<NullValueException>(() =>
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
    public void Evaluate_ContinueStatementOutsideLoop_ThrowsUnexpectedStatementException()
    {
        Assert.Throws<UnexpectedStatementException>(() =>
        {
            RazeScript.Evaluate("continue");
        });
    }

    [Fact]
    public void Evaluate_BreakStatementOutsideLoop_ThrowsUnexpectedStatementException()
    {
        Assert.Throws<UnexpectedStatementException>(() =>
        {
            RazeScript.Evaluate("break");
        });
    }
}
