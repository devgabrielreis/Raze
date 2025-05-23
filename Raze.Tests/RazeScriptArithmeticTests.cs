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

        Assert.Equal(expectedValue, result.Value);
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
