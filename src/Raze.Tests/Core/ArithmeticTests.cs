using Raze.Script.Core.Exceptions.RuntimeExceptions;

namespace Raze.Tests.Core;

public class ArithmeticTests
{
    [Theory]
    [InlineData("10 + 3 * 6", 28)]
    [InlineData("10 + (3 * 6)", 28)]
    [InlineData("(10 + 3) * 6", 78)]
    [InlineData("(10 + -3)", 7)]
    [InlineData("(10 + +3)", 13)]
    public void Evaluate_Expressions_RespectsPrecedence(string expression, int expectedValue)
    {
        RazeAssert.EvaluatesToInteger(expression, expectedValue);
    }

    [Theory]
    [InlineData("18 == 3 * 6", true)]
    [InlineData("18 * 3 == 54", true)]
    [InlineData("(true == false) == true", false)]
    [InlineData("10 > 9 == 9 > 10", false)]
    [InlineData("10 > 9 == 9 < 10", true)]
    [InlineData("true || false && false", true)]
    [InlineData("(true || false) && false", false)]
    public void Evaluate_ComparisonExpressions_RespectsPrecedence(string expression, bool expectedValue)
    {
        RazeAssert.EvaluatesToBoolean(expression, expectedValue);
    }

    [Theory]
    [InlineData("100 / 0")]
    [InlineData("100.0 / 0.0")]
    [InlineData("100 / ((100 * 2 - 199) - 1)")]
    [InlineData("100 % 0")]
    [InlineData("100.0 % 0.0")]
    public void Evaluate_DivisionByZero_ThrowsDivisionByZeroException(string expression)
    {
        RazeAssert.ReturnsError<DivisionByZeroException>(expression);
    }
}
