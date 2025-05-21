using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Values;

namespace Raze.Tests;

public class RazeScriptArithmeticTests
{
    [Theory]
    [InlineData("10 + 10", 20)]
    [InlineData("10 - 11", -1)]
    [InlineData("10 * 10", 100)]
    [InlineData("10 / 10", 1)]
    [InlineData("10 % 3", 1)]
    public void Evaluate_Expressions_ReturnsExpectedValue(string expression, int expectedValue)
    {
        var result = RazeScript.Evaluate(expression);

        Assert.IsType<IntegerValue>(result);

        Assert.Equal(expectedValue, result.Value);
    }

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
    [InlineData("10 + NULL")]
    [InlineData("NULL + 10")]
    public void Evaluate_InvalidBinaryOperations_ThrowUnsupportedBinaryOperationException(string expression)
    {
        Assert.Throws<UnsupportedBinaryOperationException>(() =>
        {
            var result = RazeScript.Evaluate(expression);
        });
    }

    [Theory]
    [InlineData("10 + 1.5")]
    [InlineData("0.5 - 10")]
    [InlineData("10 * 0.1")]
    [InlineData("100.5 / 10")]
    [InlineData("10 % 3.0")]
    public void Evaluate_OperationsBetweenIntegerAndFloat_ReturnsFloat(string expression)
    {
        Assert.IsType<FloatValue>(RazeScript.Evaluate(expression));
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
