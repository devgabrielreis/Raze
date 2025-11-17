using System.Globalization;
using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;

namespace Raze.Tests.Core.Variables;

public class DecimalVariableTests
{
    [Theory]
    [InlineData("var decimal test = 10.0", "test", "10.0")]
    [InlineData("var decimal? test = null", "test", null)]
    public void Evaluate_DecimalVariableDeclaration_ReturnsExpectedValue(string expression, string varname, string? decimalStr)
    {
        decimal? expected = decimalStr is null ? null : decimal.Parse(decimalStr, CultureInfo.InvariantCulture);
        var scope = new InterpreterScope();
        RazeScript.Evaluate(expression, scope);

        var result = RazeScript.Evaluate(varname, scope);

        if (expected is null)
        {
            Assert.IsType<NullValue>(result);
        }
        else
        {
            Assert.IsType<DecimalValue>(result);

            Assert.Equal(expected, (result as DecimalValue)!.DecValue);
        }
    }

    [Theory]
    [InlineData("var decimal test = true")]
    [InlineData("const decimal test = \"a\"")]
    [InlineData("const decimal test = null")]
    [InlineData("const decimal test = 5")]
    public void Evaluate_WrongDecimalVariableTypeAssignment_ThrowsVariableTypeException(string expression)
    {
        Assert.Throws<VariableTypeException>(() =>
        {
            var result = RazeScript.Evaluate(expression);
        });
    }
}
