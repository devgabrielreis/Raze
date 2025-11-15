using System.Globalization;
using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;

namespace Raze.Tests.Core.Types;

public class DecimalTests
{
    [Theory]
    [InlineData("10.0 + 10.0", "20.0")]
    [InlineData("10.0 - 11.0", "-1.0")]
    [InlineData("10.0 / 2.0", "5.0")]
    [InlineData("10.0 * 3.5", "35.0")]
    [InlineData("10.0 % 3.0", "1.0")]
    [InlineData("10.0 + 10", "20.0")]
    [InlineData("10.0 - 15", "-5.0")]
    [InlineData("5.0 / 2", "2.5")]
    [InlineData("10.0 * 3", "30.0")]
    [InlineData("10.0 % 3", "1.0")]
    public void Evaluate_DecimalArithmeticExpression_ReturnsExpectedValue(string expression, string decimalStr)
    {
        decimal? expected = decimalStr is null ? null : decimal.Parse(decimalStr, CultureInfo.InvariantCulture);
        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(expression, scope);

        Assert.IsType<DecimalValue>(result);
        Assert.Equal(expected, (result as DecimalValue)!.DecValue);
    }

    [Theory]
    [InlineData("10.0 == 10.0", true)]
    [InlineData("10.0 == 10", true)]
    [InlineData("10.0 == 10.1", false)]
    [InlineData("10.0 == 11", false)]
    [InlineData("10.0 != 10.0", false)]
    [InlineData("10.0 != 10", false)]
    [InlineData("10.0 != 10.1", true)]
    [InlineData("10.0 != 11", true)]
    [InlineData("10.0 > 9.9", true)]
    [InlineData("10.0 > 10.0", false)]
    [InlineData("10.0 > 10.1", false)]
    [InlineData("10.0 > 9", true)]
    [InlineData("10.0 > 10", false)]
    [InlineData("10.0 > 11", false)]
    [InlineData("10.0 < 9.9", false)]
    [InlineData("10.0 < 10.0", false)]
    [InlineData("10.0 < 10.1", true)]
    [InlineData("10.0 < 9", false)]
    [InlineData("10.0 < 10", false)]
    [InlineData("10.0 < 11", true)]
    [InlineData("10.0 >= 9.9", true)]
    [InlineData("10.0 >= 10.0", true)]
    [InlineData("10.0 >= 10.1", false)]
    [InlineData("10.0 >= 9", true)]
    [InlineData("10.0 >= 10", true)]
    [InlineData("10.0 >= 11", false)]
    [InlineData("10.0 <= 9.9", false)]
    [InlineData("10.0 <= 10.0", true)]
    [InlineData("10.0 <= 10.1", true)]
    [InlineData("10.0 <= 9", false)]
    [InlineData("10.0 <= 10", true)]
    [InlineData("10.0 <= 11", true)]
    public void Evaluate_DecimalComparisonExpression_ReturnsExpectedValue(string expression, bool expected)
    {
        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(expression, scope);

        Assert.IsType<BooleanValue>(result);
        Assert.Equal(expected, (result as BooleanValue)!.BoolValue);
    }

    [Theory]
    [InlineData("10.0 && 10.0")]
    [InlineData("10.0 || 10.0")]
    [InlineData("10.0 && 10")]
    [InlineData("10.0 || 10")]
    [InlineData("10.0 + true")]
    [InlineData("10.0 - true")]
    [InlineData("10.0 / true")]
    [InlineData("10.0 * true")]
    [InlineData("10.0 % true")]
    [InlineData("10.0 == true")]
    [InlineData("10.0 != true")]
    [InlineData("10.0 > true")]
    [InlineData("10.0 < true")]
    [InlineData("10.0 >= true")]
    [InlineData("10.0 <= true")]
    [InlineData("10.0 && true")]
    [InlineData("10.0 || true")]
    [InlineData("10.0 + null")]
    [InlineData("10.0 - null")]
    [InlineData("10.0 / null")]
    [InlineData("10.0 * null")]
    [InlineData("10.0 % null")]
    [InlineData("10.0 == null")]
    [InlineData("10.0 != null")]
    [InlineData("10.0 > null")]
    [InlineData("10.0 < null")]
    [InlineData("10.0 >= null")]
    [InlineData("10.0 <= null")]
    [InlineData("10.0 && null")]
    [InlineData("10.0 || null")]
    [InlineData("10.0 + \"a\"")]
    [InlineData("10.0 - \"a\"")]
    [InlineData("10.0 / \"a\"")]
    [InlineData("10.0 * \"a\"")]
    [InlineData("10.0 % \"a\"")]
    [InlineData("10.0 == \"a\"")]
    [InlineData("10.0 != \"a\"")]
    [InlineData("10.0 > \"a\"")]
    [InlineData("10.0 < \"a\"")]
    [InlineData("10.0 >= \"a\"")]
    [InlineData("10.0 <= \"a\"")]
    [InlineData("10.0 && \"a\"")]
    [InlineData("10.0 || \"a\"")]
    public void Evaluate_InvalidDecimalBinaryOperations_ThrowUnsupportedBinaryOperationException(string expression)
    {
        Assert.Throws<UnsupportedBinaryOperationException>(() =>
        {
            var result = RazeScript.Evaluate(expression);
        });
    }

    [Fact]
    public void Evaluate_DecimalValue_CanStartWithDot()
    {
        var script = @"
            var decimal test = .5;
            test = test + .05;
            test = test + .0;
            test = test + .00;
            test = test + .450;
            test
        ";
        decimal expectedResult = 1.0M;

        var result = RazeScript.Evaluate(script);
        Assert.Equal(expectedResult, (result as DecimalValue)!.DecValue);
    }
}
