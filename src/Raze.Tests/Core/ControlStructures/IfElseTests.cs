using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;

namespace Raze.Tests.Core.ControlStructures;

public class IfElseTests
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

        RazeAssert.EvaluatesToInteger(script, 10);
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

        RazeAssert.EvaluatesToInteger(script, 0);
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

        RazeAssert.EvaluatesToInteger(script, 20);
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

        RazeAssert.EvaluatesToInteger(script, 50);
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

        RazeAssert.ReturnsError<UnexpectedTokenException>(script);
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

        RazeAssert.ReturnsError<UnexpectedTypeException>(script);
    }

    [Fact]
    public void Evaluate_NullBooleanIfCondition_ThrowsUnexpectedTypeException()
    {
        var script = @"
            var integer test = 0;
            var boolean? nullBoolean = null;
            if (nullBoolean) {
                test = 10;
            }
            test
        ";

        RazeAssert.ReturnsError<UnexpectedTypeException>(script);
    }
}
