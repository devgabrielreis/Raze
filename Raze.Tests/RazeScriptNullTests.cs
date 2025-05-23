using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core;

namespace Raze.Tests;

public class RazeScriptNullTests
{
    [Theory]
    [InlineData("NULL + 10")]
    [InlineData("NULL - 10")]
    [InlineData("NULL / 10")]
    [InlineData("NULL * 10")]
    [InlineData("NULL % 10")]
    [InlineData("NULL + 10.0")]
    [InlineData("NULL - 10.0")]
    [InlineData("NULL / 10.0")]
    [InlineData("NULL * 10.0")]
    [InlineData("NULL % 10.0")]
    [InlineData("NULL + true")]
    [InlineData("NULL - true")]
    [InlineData("NULL / true")]
    [InlineData("NULL * true")]
    [InlineData("NULL % true")]
    [InlineData("NULL + NULL")]
    [InlineData("NULL - NULL")]
    [InlineData("NULL / NULL")]
    [InlineData("NULL * NULL")]
    [InlineData("NULL % NULL")]
    [InlineData("NULL + \"a\"")]
    [InlineData("NULL - \"a\"")]
    [InlineData("NULL / \"a\"")]
    [InlineData("NULL * \"a\"")]
    [InlineData("NULL % \"a\"")]
    public void Evaluate_NULLBinaryOperations_ThrowUnsupportedBinaryOperationException(string expression)
    {
        Assert.Throws<UnsupportedBinaryOperationException>(() =>
        {
            var result = RazeScript.Evaluate(expression);
        });
    }
}
