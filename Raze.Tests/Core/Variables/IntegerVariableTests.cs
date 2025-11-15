using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;

namespace Raze.Tests.Core.Variables;

public class IntegerVariableTests
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
