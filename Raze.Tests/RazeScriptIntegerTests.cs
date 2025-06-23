using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;
using Raze.Script.Core;
using System.Globalization;

namespace Raze.Tests;

public class RazeScriptIntegerTests
{
    [Theory]
    [InlineData("const integer test = 5", "test", 5)]
    [InlineData("var integer? test = null", "test", null)]
    public void Evaluate_IntegerVariableDeclaration_ReturnsExpectedValue(string expression, string varname, int? expected)
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate(expression, scope);

        var result = RazeScript.Evaluate(varname, scope);

        Assert.IsType<IntegerValue>(result);

        Assert.Equal(expected, (result as IntegerValue)!.IntValue);
    }

    [Theory]
    [InlineData("var integer test = true")]
    [InlineData("var integer test = 10.0")]
    [InlineData("const integer test = \"a\"")]
    [InlineData("const integer test = null")]
    public void Evaluate_WrongIntegerVariableTypeAssignment_ThrowsVariableTypeException(string expression)
    {
        Assert.Throws<VariableTypeException>(() =>
        {
            var result = RazeScript.Evaluate(expression);
        });
    }

    [Theory]
    [InlineData("10 + 10", 20)]
    [InlineData("10 - 15", -5)]
    [InlineData("6 / 2", 3)]
    [InlineData("10 * 3", 30)]
    [InlineData("10 % 3", 1)]
    public void Evaluate_IntegerArithmeticExpression_ReturnsExpectedValue(string expression, int expected)
    {
        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(expression, scope);

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(expected, (result as IntegerValue)!.IntValue);
    }

    [Theory]
    [InlineData("10 == 10", true)]
    [InlineData("10 == 10.0", true)]
    [InlineData("10 == 11", false)]
    [InlineData("10 == 10.1", false)]
    [InlineData("10 != 10", false)]
    [InlineData("10 != 10.0", false)]
    [InlineData("10 != 11", true)]
    [InlineData("10 != 10.1", true)]
    [InlineData("10 > 9", true)]
    [InlineData("10 > 10", false)]
    [InlineData("10 > 11", false)]
    [InlineData("10 > 9.9", true)]
    [InlineData("10 > 10.0", false)]
    [InlineData("10 > 10.1", false)]
    [InlineData("10 < 9", false)]
    [InlineData("10 < 10", false)]
    [InlineData("10 < 11", true)]
    [InlineData("10 < 9.9", false)]
    [InlineData("10 < 10.0", false)]
    [InlineData("10 < 10.1", true)]
    [InlineData("10 >= 9", true)]
    [InlineData("10 >= 10", true)]
    [InlineData("10 >= 11", false)]
    [InlineData("10 >= 9.9", true)]
    [InlineData("10 >= 10.0", true)]
    [InlineData("10 >= 10.1", false)]
    [InlineData("10 <= 9", false)]
    [InlineData("10 <= 10", true)]
    [InlineData("10 <= 11", true)]
    [InlineData("10 <= 9.9", false)]
    [InlineData("10 <= 10.0", true)]
    [InlineData("10 <= 10.1", true)]
    public void Evaluate_IntegerComparisonExpression_ReturnsExpectedValue(string expression, bool expected)
    {
        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(expression, scope);

        Assert.IsType<BooleanValue>(result);
        Assert.Equal(expected, (result as BooleanValue)!.BoolValue);
    }

    [Theory]
    [InlineData("10 + 10.0", "20.0")]
    [InlineData("10 - 15.0", "-5.0")]
    [InlineData("6 / 2.0", "3.0")]
    [InlineData("10 * 3.0", "30.0")]
    [InlineData("10 % 3.0", "1.0")]
    public void Evaluate_IntegerArithmeticExpressionWithDecimal_ReturnsExpectedValue(string expression, string expectedDecimalStr)
    {
        decimal expected = decimal.Parse(expectedDecimalStr, CultureInfo.InvariantCulture);

        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(expression, scope);

        Assert.IsType<DecimalValue>(result);
        Assert.Equal(expected, (result as DecimalValue)!.DecValue);
    }

    [Theory]
    [InlineData("10 && 10.0")]
    [InlineData("10 || 10.0")]
    [InlineData("10 && 10")]
    [InlineData("10 || 10")]
    [InlineData("10 + true")]
    [InlineData("10 - true")]
    [InlineData("10 / true")]
    [InlineData("10 * true")]
    [InlineData("10 % true")]
    [InlineData("10 == true")]
    [InlineData("10 != true")]
    [InlineData("10 > true")]
    [InlineData("10 < true")]
    [InlineData("10 >= true")]
    [InlineData("10 <= true")]
    [InlineData("10 && true")]
    [InlineData("10 || true")]
    [InlineData("10 + null")]
    [InlineData("10 - null")]
    [InlineData("10 / null")]
    [InlineData("10 * null")]
    [InlineData("10 % null")]
    [InlineData("10 == null")]
    [InlineData("10 != null")]
    [InlineData("10 > null")]
    [InlineData("10 < null")]
    [InlineData("10 >= null")]
    [InlineData("10 <= null")]
    [InlineData("10 && null")]
    [InlineData("10 || null")]
    [InlineData("10 + \"a\"")]
    [InlineData("10 - \"a\"")]
    [InlineData("10 / \"a\"")]
    [InlineData("10 * \"a\"")]
    [InlineData("10 % \"a\"")]
    [InlineData("10 == \"a\"")]
    [InlineData("10 != \"a\"")]
    [InlineData("10 > \"a\"")]
    [InlineData("10 < \"a\"")]
    [InlineData("10 >= \"a\"")]
    [InlineData("10 <= \"a\"")]
    [InlineData("10 && \"a\"")]
    [InlineData("10 || \"a\"")]
    public void Evaluate_InvalidIntegerBinaryOperations_ThrowUnsupportedBinaryOperationException(string expression)
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
    [InlineData("!=")]
    [InlineData(">")]
    [InlineData("<")]
    [InlineData(">=")]
    [InlineData("<=")]
    public void Evaluate_IntegerOperationWithNullIntegerVariable_ThrowsNullValueException(string op)
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate("var integer? nullVar = null", scope);

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate($"1 {op} nullVar", scope);
        });

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate($"nullVar {op} 1", scope);
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
    public void Evaluate_IntegerOperationWithNullDecimalVariable_ThrowsNullValueException(string op)
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate("var decimal? nullVar = null", scope);

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate($"1 {op} nullVar", scope);
        });

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate($"nullVar {op} 1", scope);
        });
    }
}
