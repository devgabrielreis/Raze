using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Values;

namespace Raze.Tests;

public class RazeScriptArithmeticTests
{
    [Theory]
    [InlineData("10 + 3 * 6", 28)]
    [InlineData("10 + (3 * 6)", 28)]
    [InlineData("(10 + 3) * 6", 78)]
    public void Evaluate_Expressions_RespectsPrecedence(string expression, int expectedValue)
    {
        var result = RazeScript.Evaluate(expression);

        Assert.IsType<IntegerValue>(result);

        Assert.Equal(expectedValue, (result as IntegerValue)!.IntValue);
    }

    [Theory]
    [InlineData("18 == 3 * 6", true)]
    [InlineData("18 * 3 == 54", true)]
    [InlineData("(true == false) == true", false)]
    [InlineData("10 > 9 == 9 > 10", false)]
    public void Evaluate_ComparisonExpressions_RespectsPrecedence(string expression, bool expectedValue)
    {
        var result = RazeScript.Evaluate(expression);

        Assert.IsType<BooleanValue>(result);

        Assert.Equal(expectedValue, (result as BooleanValue)!.BoolValue);
    }

    [Theory]
    [InlineData("100 / 0")]
    [InlineData("100 / 0.0")]
    [InlineData("100.0 / 0")]
    [InlineData("100.0 / 0.0")]
    [InlineData("100 / ((100 * 2 - 199) - 1)")]
    public void Evaluate_DivisionByZero_ThrowsDivisionByZeroException(string expression)
    {
        Assert.Throws<DivisionByZeroException>(() =>
        {
            RazeScript.Evaluate(expression);
        });
    }
}
