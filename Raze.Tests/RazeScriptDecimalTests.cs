using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;
using Raze.Script.Core;
using System.Globalization;

namespace Raze.Tests;

public class RazeScriptDecimalTests
{
    [Theory]
    [InlineData("var decimal test = 10.0", "test", "10.0")]
    [InlineData("const decimal test = 5", "test", "5.0")]
    [InlineData("var decimal test = null", "test", null)]
    [InlineData("var decimal test", "test", null)]
    public void Evaluate_DecimalVariableDeclaration_ReturnsExpectedValue(string expression, string varname, string? decimalStr)
    {
        decimal? expected = decimalStr is null ? null : decimal.Parse(decimalStr, CultureInfo.InvariantCulture);
        var scope = new InterpreterScope();
        RazeScript.Evaluate(expression, scope);

        var result = RazeScript.Evaluate(varname, scope);

        Assert.IsType<DecimalValue>(result);

        Assert.Equal(expected, (result as DecimalValue)!.DecValue);
    }

    [Theory]
    [InlineData("var decimal test = true")]
    [InlineData("const decimal test = \"a\"")]
    public void Evaluate_WrongDecimalVariableTypeAssignment_ThrowsVariableTypeException(string expression)
    {
        Assert.Throws<VariableTypeException>(() =>
        {
            var result = RazeScript.Evaluate(expression);
        });
    }

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
    public void Evaluate_DecimalComparisonExpression_ReturnsExpectedValue(string expression, bool expected)
    {
        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(expression, scope);

        Assert.IsType<BooleanValue>(result);
        Assert.Equal(expected, (result as BooleanValue)!.BoolValue);
    }

    [Theory]
    [InlineData("10.0 + true")]
    [InlineData("10.0 - true")]
    [InlineData("10.0 / true")]
    [InlineData("10.0 * true")]
    [InlineData("10.0 % true")]
    [InlineData("10.0 == true")]
    [InlineData("10.0 + null")]
    [InlineData("10.0 - null")]
    [InlineData("10.0 / null")]
    [InlineData("10.0 * null")]
    [InlineData("10.0 % null")]
    [InlineData("10.0 == null")]
    [InlineData("10.0 + \"a\"")]
    [InlineData("10.0 - \"a\"")]
    [InlineData("10.0 / \"a\"")]
    [InlineData("10.0 * \"a\"")]
    [InlineData("10.0 % \"a\"")]
    [InlineData("10.0 == \"a\"")]
    public void Evaluate_InvalidDecimalBinaryOperations_ThrowUnsupportedBinaryOperationException(string expression)
    {
        Assert.Throws<UnsupportedBinaryOperationException>(() =>
        {
            var result = RazeScript.Evaluate(expression);
        });
    }

    [Theory]
    [InlineData("+")]
    [InlineData("-")]
    [InlineData("*")]
    [InlineData("/")]
    [InlineData("%")]
    [InlineData("==")]
    public void Evaluate_DecimalOperationWithNullDecimalVariable_ThrowsNullValueException(string op)
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate("var decimal nullVar", scope);

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate($"1.0 {op} nullVar", scope);
        });

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate($"nullVar {op} 1.0", scope);
        });
    }

    [Theory]
    [InlineData("+")]
    [InlineData("-")]
    [InlineData("*")]
    [InlineData("/")]
    [InlineData("%")]
    [InlineData("==")]
    public void Evaluate_DecimalOperationWithNullIntegerVariable_ThrowsNullValueException(string op)
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate("var integer nullVar", scope);

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate($"1.0 {op} nullVar", scope);
        });

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate($"nullVar {op} 1.0", scope);
        });
    }
}
