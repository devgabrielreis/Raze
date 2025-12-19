using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core;

namespace Raze.Tests.Core.Types;

public class NullTests
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
    [InlineData("null && 10")]
    [InlineData("null || 10")]
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
    [InlineData("null && 10.0")]
    [InlineData("null || 10.0")]
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
    [InlineData("null && true")]
    [InlineData("null || true")]
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
    [InlineData("null && null")]
    [InlineData("null || null")]
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
    [InlineData("null && \"a\"")]
    [InlineData("null || \"a\"")]
    public void Evaluate_NullBinaryOperations_ThrowUnsupportedBinaryOperationException(string expression)
    {
        Assert.Throws<UnsupportedBinaryOperationException>(() =>
        {
            var result = RazeScript.Evaluate(expression, "Raze.Tests");
        });
    }

    [Theory]
    [InlineData("null++")]
    [InlineData("null--")]
    [InlineData("-null")]
    [InlineData("+null")]
    [InlineData("!null")]
    public void Evaluate_InvalidNullUnaryOperations_ThrowUnsupportedUnaryOperationException(string expression)
    {
        Assert.Throws<UnsupportedUnaryOperationException>(() =>
        {
            var result = RazeScript.Evaluate(expression, "Raze.Tests");
        });
    }
}
