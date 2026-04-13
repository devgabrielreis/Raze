using System.Globalization;

namespace Raze.Tests.Core.BuiltInModules;

public class StringsModuleTests
{
    [Theory]
    [InlineData(11, "11")]
    [InlineData(-11, "-11")]
    [InlineData(0, "0")]
    public void Evaluate_IntegerToStringFunction_ReturnsExpectedValue(int value, string expected)
    {
        var script = $"""
            import strings;

            strings::integerToString({value})
        """;

        RazeAssert.EvaluatesToString(script, expected);
    }

    [Theory]
    [InlineData("11.0", "11.0")]
    [InlineData("-11.0", "-11.0")]
    [InlineData("0.0", "0.0")]
    [InlineData("0.1234", "0.1234")]
    [InlineData("1.1000", "1.1")]
    public void Evaluate_DecimalToStringFunction_ReturnsExpectedValue(string decimalStr, string expected)
    {
        var script = $"""
            import strings;

            strings::decimalToString({decimalStr})
        """;

        RazeAssert.EvaluatesToString(script, expected);
    }

    [Theory]
    [InlineData(true, "1", "0")]
    [InlineData(false, "Yes", "No")]
    [InlineData(true, ":)", ":(")]
    [InlineData(false, "big", "")]
    public void Evaluate_BooleanToStringFunction_ReturnsExpectedValue(bool value, string ifTrue, string ifFalse)
    {
        var script = $"""
            import strings;

            strings::booleanToString({(value ? "true" : "false")}, "{ifTrue}", "{ifFalse}")
        """;

        RazeAssert.EvaluatesToString(script, value ? ifTrue : ifFalse);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("-1", -1)]
    [InlineData("0", 0)]
    [InlineData("1222", 1222)]
    [InlineData("1222.0", null)]
    [InlineData("1a", null)]
    [InlineData("a1", null)]
    [InlineData("", null)]
    [InlineData("two", null)]
    public void Evaluate_TryStringToIntegerFunction_ReturnsExpectedValue(string str, int? expected)
    {
        var script = $"""
            import strings;

            strings::tryStringToInteger("{str}")
        """;

        if (expected == null)
        {
            RazeAssert.EvaluatesToNull(script);
        }
        else
        {
            RazeAssert.EvaluatesToInteger(script, expected.Value);
        }
    }

    [Theory]
    [InlineData("1.0", "1.0")]
    [InlineData("-1.01", "-1.01")]
    [InlineData("0.0", "0.0")]
    [InlineData("12.22", "12.22")]
    [InlineData("1222", "1222.0")]
    [InlineData("1a", null)]
    [InlineData("a1", null)]
    [InlineData("", null)]
    [InlineData("two", null)]
    [InlineData(".7", "0.7")]
    [InlineData("7.", null)]
    [InlineData("7.0a", null)]
    public void Evaluate_TryStringToDecimalFunction_ReturnsExpectedValue(string str, string? expectedStr)
    {
        var script = $"""
            import strings;

            strings::tryStringToDecimal("{str}")
        """;

        if (expectedStr == null)
        {
            RazeAssert.EvaluatesToNull(script);
        }
        else
        {
            decimal expected = decimal.Parse(expectedStr, CultureInfo.InvariantCulture);
            RazeAssert.EvaluatesToDecimal(script, expected);
        }
    }
}
