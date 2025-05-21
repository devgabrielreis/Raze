using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;

namespace Raze.Tests;

public class RazeScriptArithmeticTests
{
    [Theory]
    [InlineData("10 + 10", 1)]
    [InlineData("10 - 11", -1)]
    [InlineData("10 * 10", 100)]
    [InlineData("10 / 10", 1)]
    [InlineData("10 % 3", 1)]
    public void Evaluate_Expressions_ReturnsExpectedValue(string expression, int expectedValue)
    {
        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(expression, scope);

        Assert.IsType<IntegerValue>(result);

        Assert.Equal(expectedValue, result.Value);
    }

    [Theory]
    [InlineData("10 + 3 * 6", 28)]
    [InlineData("10 + (3 * 6)", 28)]
    [InlineData("(10 + 3) * 6", 78)]
    public void Evaluate_Expressions_RespectsPrecedence(string expression, int expectedValue)
    {
        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(expression, scope);

        Assert.IsType<IntegerValue>(result);

        Assert.Equal(expectedValue, result.Value);
    }

    [Theory]
    [InlineData("10 + NULL")]
    [InlineData("NULL + 10")]
    public void Evaluate_InvalidBinaryOperations_ThrowUnsupportedBinaryOperationException(string expression)
    {
        var scope = new InterpreterScope();

        Assert.Throws<UnsupportedBinaryOperationException>(() =>
        {
            var result = RazeScript.Evaluate(expression, scope);
        });
    }

    [Fact]
    public void Evaluate_UninitializedOrNullVariable_ThrowsNullValueException()
    {
        var scope = new InterpreterScope();

        RazeScript.Evaluate("var integer a = NULL", scope);

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate("10 + a", scope);
        });

        RazeScript.Evaluate("const integer b", scope);

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate("10 + b", scope);
        });
    }
}
