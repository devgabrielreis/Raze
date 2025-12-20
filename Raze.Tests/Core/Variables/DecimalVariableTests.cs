using System.Globalization;
using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Runtime.Scopes;
using Raze.Script.Core.Runtime.Values;

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
        RazeScript.Evaluate(expression, "Raze.Tests", scope);

        var result = RazeScript.Evaluate(varname, "Raze.Tests", scope);

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
            var result = RazeScript.Evaluate(expression, "Raze.Tests");
        });
    }

    [Theory]
    [InlineData("++", false, "10.1", "11.1", "11.1")]
    [InlineData("++", true, "10.2", "10.2", "11.2")]
    [InlineData("--", false, "10.3", "9.3", "9.3")]
    [InlineData("--", true, "10.4", "10.4", "9.4")]
    public void Evaluate_UnaryMutationOperator_ReturnsExpectedValueAndChangeVariable(
        string op, bool isPostfix, string variableInitualValueStr, string expressionResultStr, string variableAfterValueStr
    )
    {
        var script = $@"
            var decimal test = {variableInitualValueStr};
            {(isPostfix ? $"test{op}" : $"{op}test")}
        ";

        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(script, "Raze.Tests", scope);

        Assert.IsType<DecimalValue>(result);
        Assert.Equal(expressionResultStr, result.ToString());

        result = RazeScript.Evaluate("test", "Raze.Tests", scope);

        Assert.IsType<DecimalValue>(result);
        Assert.Equal(variableAfterValueStr, result.ToString());
    }
}
