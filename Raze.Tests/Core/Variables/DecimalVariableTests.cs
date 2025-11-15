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
    [InlineData("const decimal test = 5", "test", "5.0")]
    [InlineData("var decimal? test = null", "test", null)]
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
    [InlineData("const decimal test = null")]
    public void Evaluate_WrongDecimalVariableTypeAssignment_ThrowsVariableTypeException(string expression)
    {
        Assert.Throws<VariableTypeException>(() =>
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
    [InlineData("!=")]
    [InlineData(">")]
    [InlineData("<")]
    [InlineData(">=")]
    [InlineData("<=")]
    public void Evaluate_DecimalOperationWithNullDecimalVariable_ThrowsNullValueException(string op)
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate("var decimal? nullVar = null", scope);

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
    [InlineData("!=")]
    [InlineData(">")]
    [InlineData("<")]
    [InlineData(">=")]
    [InlineData("<=")]
    public void Evaluate_DecimalOperationWithNullIntegerVariable_ThrowsNullValueException(string op)
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate("var integer? nullVar = null", scope);

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
