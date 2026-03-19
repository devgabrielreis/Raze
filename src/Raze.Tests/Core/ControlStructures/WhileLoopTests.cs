using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;

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

        RazeAssert.EvaluatesToInteger(script, 12);
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

        RazeAssert.ReturnsError<UnexpectedTypeException>(script);
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

        RazeAssert.ReturnsError<UnexpectedTypeException>(script);
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

        RazeAssert.EvaluatesToInteger(script, 5);
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

        RazeAssert.EvaluatesToInteger(script, 12);
    }

    [Fact]
    public void Evaluate_EmptyWhileLoopCondition_ThrowsUnexpectedTokenException()
    {
        var script = @"
            while () {
                break;
            }
        ";

        RazeAssert.ReturnsError<UnexpectedTokenException>(script);
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

        RazeAssert.EvaluatesToInteger(script, 10);
    }
}
