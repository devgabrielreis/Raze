using Raze.Script.Core.Exceptions.RuntimeExceptions;
using System.Globalization;

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
        decimal expected = decimal.Parse(decimalStr, CultureInfo.InvariantCulture);
        RazeAssert.EvaluatesToDecimal(expression, expected);
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
        RazeAssert.EvaluatesToBoolean(expression, expected);
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
        RazeAssert.ReturnsError<UnsupportedBinaryOperationException>(expression);
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
        RazeAssert.EvaluatesToDecimal(script, expectedResult);
    }

    [Theory]
    [InlineData("+10.1", "10.1")]
    [InlineData("-10.2", "-10.2")]
    public void Evaluate_DecimalUnaryOperation_ReturnsExpectedValue(string expression, string expectedStr)
    {
        decimal expected = decimal.Parse(expectedStr, CultureInfo.InvariantCulture);
        RazeAssert.EvaluatesToDecimal(expression, expected);
    }

    [Theory]
    [InlineData("!10.0")]
    public void Evaluate_InvalidDecimalUnaryOperations_ThrowUnsupportedUnaryOperationException(string expression)
    {
        RazeAssert.ReturnsError<UnsupportedUnaryOperationException>(expression);
    }
}
