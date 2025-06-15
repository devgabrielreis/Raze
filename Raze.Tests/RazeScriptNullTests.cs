using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core;

namespace Raze.Tests;

public class RazeScriptNullTests
{
    [Theory]
    [InlineData("null + 10")]
    [InlineData("null - 10")]
    [InlineData("null / 10")]
    [InlineData("null * 10")]
    [InlineData("null % 10")]
    [InlineData("null == 10")]
    [InlineData("null != 10")]
    [InlineData("null > 10")]
    [InlineData("null < 10")]
    [InlineData("null >= 10")]
    [InlineData("null <= 10")]
    [InlineData("null + 10.0")]
    [InlineData("null - 10.0")]
    [InlineData("null / 10.0")]
    [InlineData("null * 10.0")]
    [InlineData("null % 10.0")]
    [InlineData("null == 10.0")]
    [InlineData("null != 10.0")]
    [InlineData("null > 10.0")]
    [InlineData("null < 10.0")]
    [InlineData("null >= 10.0")]
    [InlineData("null <= 10.0")]
    [InlineData("null + true")]
    [InlineData("null - true")]
    [InlineData("null / true")]
    [InlineData("null * true")]
    [InlineData("null % true")]
    [InlineData("null == true")]
    [InlineData("null != true")]
    [InlineData("null > true")]
    [InlineData("null < true")]
    [InlineData("null >= true")]
    [InlineData("null <= true")]
    [InlineData("null + null")]
    [InlineData("null - null")]
    [InlineData("null / null")]
    [InlineData("null * null")]
    [InlineData("null % null")]
    [InlineData("null == null")]
    [InlineData("null != null")]
    [InlineData("null > null")]
    [InlineData("null < null")]
    [InlineData("null >= null")]
    [InlineData("null <= null")]
    [InlineData("null + \"a\"")]
    [InlineData("null - \"a\"")]
    [InlineData("null / \"a\"")]
    [InlineData("null * \"a\"")]
    [InlineData("null % \"a\"")]
    [InlineData("null == \"a\"")]
    [InlineData("null != \"a\"")]
    [InlineData("null > \"a\"")]
    [InlineData("null < \"a\"")]
    [InlineData("null >= \"a\"")]
    [InlineData("null <= \"a\"")]
    public void Evaluate_NullBinaryOperations_ThrowUnsupportedBinaryOperationException(string expression)
    {
        Assert.Throws<UnsupportedBinaryOperationException>(() =>
        {
            var result = RazeScript.Evaluate(expression);
        });
    }
}
