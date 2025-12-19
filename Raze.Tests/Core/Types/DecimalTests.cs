using System.Globalization;
using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Runtime.Scopes;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Tests.Core.Types;

public class DecimalTests
{
    [Theory]
    [InlineData("10.0 + 10.0", "20.0")]
    [InlineData("10.0 - 11.0", "-1.0")]
    [InlineData("10.0 / 2.0", "5.0")]
    [InlineData("10.0 * 3.5", "35.0")]
    [InlineData("10.0 % 3.0", "1.0")]
    public void Evaluate_DecimalArithmeticExpression_ReturnsExpectedValue(string expression, string decimalStr)
    {
        decimal? expected = decimalStr is null ? null : decimal.Parse(decimalStr, CultureInfo.InvariantCulture);
        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(expression, "Raze.Tests", scope);

        Assert.IsType<DecimalValue>(result);
        Assert.Equal(expected, (result as DecimalValue)!.DecValue);
    }

    [Theory]
    [InlineData("10.0 == 10.0", true)]
    [InlineData("10.0 == 10.1", false)]
    [InlineData("10.0 != 10.0", false)]
    [InlineData("10.0 != 10.1", true)]
    [InlineData("10.0 > 9.9", true)]
    [InlineData("10.0 > 10.0", false)]
    [InlineData("10.0 > 10.1", false)]
    [InlineData("10.0 < 9.9", false)]
    [InlineData("10.0 < 10.0", false)]
    [InlineData("10.0 < 10.1", true)]
    [InlineData("10.0 >= 9.9", true)]
    [InlineData("10.0 >= 10.0", true)]
    [InlineData("10.0 >= 10.1", false)]
    [InlineData("10.0 <= 9.9", false)]
    [InlineData("10.0 <= 10.0", true)]
    [InlineData("10.0 <= 10.1", true)]
    public void Evaluate_DecimalComparisonExpression_ReturnsExpectedValue(string expression, bool expected)
    {
        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(expression, "Raze.Tests", scope);

        Assert.IsType<BooleanValue>(result);
        Assert.Equal(expected, (result as BooleanValue)!.BoolValue);
    }

    [Theory]
    [InlineData("10.0 && 10.0")]
    [InlineData("10.0 || 10.0")]
    [InlineData("10.0 + 10")]
    [InlineData("10.0 - 15")]
    [InlineData("5.0 / 2")]
    [InlineData("10.0 * 3")]
    [InlineData("10.0 % 3")]
    [InlineData("10.0 && 10")]
    [InlineData("10.0 || 10")]
    [InlineData("10.0 == 10")]
    [InlineData("10.0 != 10")]
    [InlineData("10.0 > 9")]
    [InlineData("10.0 < 9")]
    [InlineData("10.0 >= 9")]
    [InlineData("10.0 <= 9")]
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
            var result = RazeScript.Evaluate(expression, "Raze.Tests");
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

        var result = RazeScript.Evaluate(script, "Raze.Tests");
        Assert.Equal(expectedResult, (result as DecimalValue)!.DecValue);
    }

    [Theory]
    [InlineData("+10.1", "10.1")]
    [InlineData("-10.2", "-10.2")]
    [InlineData("10.3++", "11.3")]
    [InlineData("10.4--", "9.4")]
    public void Evaluate_DecimalUnaryOperation_ReturnsExpectedValue(string expression, string expectedStr)
    {
        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(expression, "Raze.Tests", scope);

        Assert.IsType<DecimalValue>(result);
        Assert.Equal(expectedStr, (result as DecimalValue)!.ToString());
    }

    [Theory]
    [InlineData("!10.0")]
    public void Evaluate_InvalidDecimalUnaryOperations_ThrowUnsupportedUnaryOperationException(string expression)
    {
        Assert.Throws<UnsupportedUnaryOperationException>(() =>
        {
            var result = RazeScript.Evaluate(expression, "Raze.Tests");
        });
    }
}
