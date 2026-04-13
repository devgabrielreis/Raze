using System.Globalization;

namespace Raze.Tests.Core.BuiltInModules;

public class MathModuleTests
{
    [Fact]
    public void Evaluate_PiConstant_ReturnsExpectedValue()
    {
        var script = """
            import math;
            
            math::PI
        """;

        RazeAssert.EvaluatesToDecimal(script, 3.14159m);
    }

    [Fact]
    public void Evaluate_EConstant_ReturnsExpectedValue()
    {
        var script = """
            import math;
            
            math::E
        """;

        RazeAssert.EvaluatesToDecimal(script, 2.71828m);
    }

    [Fact]
    public void Evaluate_MaxIntegerConstant_ReturnsExpectedValue()
    {
        var script = """
            import math;
            
            math::MAX_INTEGER
        """;

        RazeAssert.EvaluatesToInteger(script, Int128.MaxValue);
    }

    [Fact]
    public void Evaluate_MinIntegerConstant_ReturnsExpectedValue()
    {
        var script = """
            import math;
            
            math::MIN_INTEGER
        """;

        RazeAssert.EvaluatesToInteger(script, Int128.MinValue);
    }

    [Fact]
    public void Evaluate_MaxDecimalConstant_ReturnsExpectedValue()
    {
        var script = """
            import math;
            
            math::MAX_DECIMAL
        """;

        RazeAssert.EvaluatesToDecimal(script, decimal.MaxValue);
    }

    [Fact]
    public void Evaluate_MinDecimalConstant_ReturnsExpectedValue()
    {
        var script = """
            import math;
            
            math::MIN_DECIMAL
        """;

        RazeAssert.EvaluatesToDecimal(script, decimal.MinValue);
    }

    [Theory]
    [InlineData(1, "1.0")]
    [InlineData(0, "0.0")]
    [InlineData(-12, "-12.0")]
    public void Evaluate_IntegerToDecimalFunction_ReturnsExpectedValue(int num, string decimalStr)
    {
        var script = $"""
            import math;

            math::integerToDecimal({num})
        """;

        decimal expected = decimal.Parse(decimalStr, CultureInfo.InvariantCulture);
        RazeAssert.EvaluatesToDecimal(script, expected);
    }

    [Theory]
    [InlineData("1.0", 1)]
    [InlineData("0.1", 1)]
    [InlineData("-12.5", -12)]
    [InlineData("-12.00001", -12)]
    public void Evaluate_CeilFunction_ReturnsExpectedValue(string numStr, int expected)
    {
        var script = $"""
            import math;

            math::ceil({numStr})
        """;

        RazeAssert.EvaluatesToInteger(script, expected);
    }

    [Theory]
    [InlineData("1.0", 1)]
    [InlineData("0.1", 0)]
    [InlineData("-12.5", -13)]
    [InlineData("-12.00001", -13)]
    public void Evaluate_FloorFunction_ReturnsExpectedValue(string numStr, int expected)
    {
        var script = $"""
            import math;

            math::floor({numStr})
        """;

        RazeAssert.EvaluatesToInteger(script, expected);
    }

    [Theory]
    [InlineData("1.0", 1)]
    [InlineData("0.1", 0)]
    [InlineData("-12.5", -12)]
    [InlineData("12.00001", 12)]
    [InlineData("0.5", 0)]
    [InlineData("1.5", 2)]
    public void Evaluate_RoundFunction_ReturnsExpectedValue(string numStr, int expected)
    {
        var script = $"""
            import math;

            math::round({numStr})
        """;

        RazeAssert.EvaluatesToInteger(script, expected);
    }

    [Theory]
    [InlineData("1.111", 1, "1.1")]
    [InlineData("1.111", 2, "1.11")]
    [InlineData("1.116", 2, "1.12")]
    [InlineData("1.116", 0, "1.0")]
    [InlineData("1.116", -1, "1.0")]
    [InlineData("1.1234567890123456789012345678", 27, "1.123456789012345678901234568")]
    [InlineData("1.1234567890123456789012345678", 28, "1.1234567890123456789012345678")]
    [InlineData("1.1234567890123456789012345678", 29, "1.1234567890123456789012345678")]
    [InlineData("1.0", 2, "1.0")]
    public void Evaluate_RoundToDecimalPlaces_ReturnsExpectedValue(string numStr, int decimalPlaces, string expectedStr)
    {
        var script = $"""
            import math;

            math::roundToDecimalPlaces({numStr}, {decimalPlaces})
        """;

        decimal expected = decimal.Parse(expectedStr, CultureInfo.InvariantCulture);
        RazeAssert.EvaluatesToDecimal(script, expected);
    }

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public void Evaluate_BooleanToInteger_ReturnsExpectedValue(bool value, int expected)
    {
        var script = $"""
            import math;

            math::booleanToInteger({(value ? "true" : "false")})
        """;

        RazeAssert.EvaluatesToInteger(script, expected);
    }

    [Theory]
    [InlineData(1, 3, 3)]
    [InlineData(-3, 3, 3)]
    [InlineData(-3, -4, -3)]
    public void Evaluate_MaxIntegerFunction_ReturnsExpectedValue(int num1, int num2, int expected)
    {
        var script = $"""
            import math;

            math::maxInteger({num1}, {num2})
        """;

        RazeAssert.EvaluatesToInteger(script, expected);
    }

    [Theory]
    [InlineData("1.0", "3.0", "3.0")]
    [InlineData("-3.0", "3.0", "3.0")]
    [InlineData("-3.0", "-4.0", "-3.0")]
    [InlineData("1.0", "1.00001", "1.00001")]
    public void Evaluate_MaxDecimalFunction_ReturnsExpectedValue(string num1Str, string num2Str, string expectedStr)
    {
        var script = $"""
            import math;

            math::maxDecimal({num1Str}, {num2Str})
        """;

        decimal expected = decimal.Parse(expectedStr, CultureInfo.InvariantCulture);
        RazeAssert.EvaluatesToDecimal(script, expected);
    }

    [Theory]
    [InlineData(1, 3, 1)]
    [InlineData(-3, 3, -3)]
    [InlineData(-3, -4, -4)]
    public void Evaluate_MinIntegerFunction_ReturnsExpectedValue(int num1, int num2, int expected)
    {
        var script = $"""
            import math;

            math::minInteger({num1}, {num2})
        """;

        RazeAssert.EvaluatesToInteger(script, expected);
    }

    [Theory]
    [InlineData("1.0", "3.0", "1.0")]
    [InlineData("-3.0", "3.0", "-3.0")]
    [InlineData("-3.0", "-4.0", "-4.0")]
    [InlineData("1.0", "1.00001", "1.0")]
    public void Evaluate_MinDecimalFunction_ReturnsExpectedValue(string num1Str, string num2Str, string expectedStr)
    {
        var script = $"""
            import math;

            math::minDecimal({num1Str}, {num2Str})
        """;

        decimal expected = decimal.Parse(expectedStr, CultureInfo.InvariantCulture);
        RazeAssert.EvaluatesToDecimal(script, expected);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(-1, 1)]
    [InlineData(0, 0)]
    public void Evaluate_AbsIntegerFunction_ReturnsExpectedValue(int num, int expected)
    {
        var script = $"""
            import math;

            math::absInteger({num})
        """;

        RazeAssert.EvaluatesToInteger(script, expected);
    }

    [Theory]
    [InlineData("1.0", "1.0")]
    [InlineData("-1.0", "1.0")]
    [InlineData("0.0", "0.0")]
    [InlineData("-0.000001", "0.000001")]
    public void Evaluate_AbsDecimalFunction_ReturnsExpectedValue(string numStr, string expectedStr)
    {
        var script = $"""
            import math;

            math::absDecimal({numStr})
        """;

        decimal expected = decimal.Parse(expectedStr, CultureInfo.InvariantCulture);
        RazeAssert.EvaluatesToDecimal(script, expected);
    }

    [Theory]
    [InlineData(1, 1, 1, 1)]
    [InlineData(1, 2, 3, 2)]
    [InlineData(1, -5, -4, -4)]
    [InlineData(1, -3, 3, 1)]
    public void Evaluate_ClampIntegerFunction_ReturnsExpectedValue(int value, int min, int max, int expected)
    {
        var script = $"""
            import math;

            math::clampInteger({value}, {min}, {max})
        """;

        RazeAssert.EvaluatesToInteger(script, expected);
    }

    [Theory]
    [InlineData("1.0", "1.0", "1.0", "1.0")]
    [InlineData("1.0", "2.0", "3.0", "2.0")]
    [InlineData("1.0", "-5.0", "-4.0", "-4.0")]
    [InlineData("1.0", "-3.0", "3.0", "1.0")]
    [InlineData("1.001", "1.002", "1.003", "1.002")]
    public void Evaluate_ClampDecimalFunction_ReturnsExpectedValue(string valueStr, string minStr, string maxStr, string expectedStr)
    {
        var script = $"""
            import math;

            math::clampDecimal({valueStr}, {minStr}, {maxStr})
        """;

        decimal expected = decimal.Parse(expectedStr, CultureInfo.InvariantCulture);
        RazeAssert.EvaluatesToDecimal(script, expected);
    }
}
