using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core;
using Raze.Script.Core.Values;
using System.Globalization;

namespace Raze.Tests;

public class RazeScriptVariableTests
{
    [Theory]
    [InlineData("var integer test = 10.0")]
    [InlineData("var integer test = 10 + 1.0")]
    public void Evaluate_WrongVariableTypeAssignment_ThrowsVariableTypeException(string expression)
    {
        Assert.Throws<VariableTypeException>(() =>
        {
            var result = RazeScript.Evaluate(expression);
        });
    }

    [Theory]
    [InlineData("var integer test = 10", "test", 10)]
    [InlineData("const integer test = 10", "test", 10)]
    [InlineData("var integer test = 10 + 10", "test", 20)]
    [InlineData("const integer test = 10 * 10", "test", 100)]
    [InlineData("var integer test", "test", null)]
    [InlineData("const integer test", "test", null)]
    public void Evaluate_IntegerDeclaration_ReturnsExpectedValue(string expression, string varname, int? expected)
    {
        TestVariableDeclaration<IntegerValue, int?>(expression, varname, expected);
    }

    [Theory]
    [InlineData("var decimal test = 10", "test", "10.0")]
    [InlineData("const decimal test = 10", "test", "10.0")]
    [InlineData("var decimal test = 10 + 10", "test", "20.0")]
    [InlineData("const decimal test = 10 * 10", "test", "100.0")]
    [InlineData("const decimal test = 13.0", "test", "13.0")]
    [InlineData("var decimal test", "test", null)]
    [InlineData("const decimal test", "test", null)]
    public void Evaluate_DecimalDeclaration_ReturnsExpectedValue(string expression, string varname, string? expectedDecimalStr)
    {
        decimal? expected = expectedDecimalStr is null ? null : decimal.Parse(expectedDecimalStr, CultureInfo.InvariantCulture);
        TestVariableDeclaration<DecimalValue, decimal?>(expression, varname, expected);
    }

    [Fact]
    public void Evaluate_UninitializedOrNullVariable_ThrowsNullValueException()
    {
        var scope = new InterpreterScope();

        RazeScript.Evaluate("var integer a = NULL", scope);

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate("10 + a", scope);
        });

        RazeScript.Evaluate("const integer b", scope);

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate("10 + b", scope);
        });
    }

    [Fact]
    public void Evaluate_ConstantAssignment_ThrowsConstantAssignmentException()
    {
        var scope = new InterpreterScope();

        RazeScript.Evaluate("const integer test = 10", scope);

        Assert.Throws<ConstantAssignmentException>(() =>
        {
            RazeScript.Evaluate("test = 11", scope);
        });
    }

    private static void TestVariableDeclaration<T, T2>(string expression, string varname, T2 expected) where T : RuntimeValue
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate(expression, scope);

        var result = RazeScript.Evaluate(varname, scope);

        Assert.IsType<T>(result);

        Assert.Equal(expected, (result as T)!.Value);
    }
}
