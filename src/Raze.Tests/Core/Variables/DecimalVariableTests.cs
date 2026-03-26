using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using System.Globalization;

namespace Raze.Tests.Core.Variables;

public class DecimalVariableTests
{
    [Theory]
    [InlineData("var decimal test = 10.0", "test", "10.0")]
    [InlineData("var decimal? test = null", "test", null)]
    public void Evaluate_DecimalVariableDeclaration_ReturnsExpectedValue(string expression, string varname, string? decimalStr)
    {
        decimal? expected = decimalStr is null ? null : decimal.Parse(decimalStr, CultureInfo.InvariantCulture);

        var scope = RazeScript.CreateInterpreterScope();
        RazeAssert.EvaluatesToVoid(expression, scope);

        if (expected is null)
        {
            RazeAssert.EvaluatesToNull(varname, scope);
        }
        else
        {
            RazeAssert.EvaluatesToDecimal(varname, expected.Value, scope);
        }
    }

    [Theory]
    [InlineData("var decimal test = true")]
    [InlineData("const decimal test = \"a\"")]
    [InlineData("const decimal test = null")]
    [InlineData("const decimal test = 5")]
    public void Evaluate_WrongDecimalVariableTypeAssignment_ThrowsVariableTypeException(string expression)
    {
        RazeAssert.ReturnsError<VariableTypeException>(expression);
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

        var scope = RazeScript.CreateInterpreterScope();

        decimal expressionResult = decimal.Parse(expressionResultStr, CultureInfo.InvariantCulture);
        RazeAssert.EvaluatesToDecimal(script, expressionResult, scope);

        decimal variableAfterValue = decimal.Parse(variableAfterValueStr, CultureInfo.InvariantCulture);
        RazeAssert.EvaluatesToDecimal("test", variableAfterValue, scope);
    }
}
