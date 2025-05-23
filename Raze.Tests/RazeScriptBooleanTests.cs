using Raze.Script.Core.Scopes;
using Raze.Script.Core;
using Raze.Script.Core.Values;
using Raze.Script.Core.Exceptions.RuntimeExceptions;

namespace Raze.Tests;

public class RazeScriptBooleanTests
{
    [Theory]
    [InlineData("var boolean test = true", "test", true)]
    [InlineData("const boolean test = false", "test", false)]
    [InlineData("var boolean test = NULL", "test", null)]
    [InlineData("var boolean test", "test", null)]
    public void Evaluate_BooleanVariableDeclaration_ReturnsExpectedValue(string expression, string varname, bool? expected)
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate(expression, scope);

        var result = RazeScript.Evaluate(varname, scope);

        Assert.IsType<BooleanValue>(result);

        Assert.Equal(expected, (bool?)((result as BooleanValue)!.Value)!);
    }

    [Theory]
    [InlineData("var boolean test = 10")]
    [InlineData("var boolean test = 10.0")]
    [InlineData("const boolean test = \"a\"")]
    public void Evaluate_WrongBooleanVariableTypeAssignment_ThrowsVariableTypeException(string expression)
    {
        Assert.Throws<VariableTypeException>(() =>
        {
            var result = RazeScript.Evaluate(expression);
        });
    }

    [Theory]
    [InlineData("true + 10")]
    [InlineData("false - 10")]
    [InlineData("true / 10")]
    [InlineData("false * 10")]
    [InlineData("true % 10")]
    [InlineData("false + 10.0")]
    [InlineData("true - 10.0")]
    [InlineData("false / 10.0")]
    [InlineData("true * 10.0")]
    [InlineData("false % 10.0")]
    [InlineData("true + NULL")]
    [InlineData("false - NULL")]
    [InlineData("true / NULL")]
    [InlineData("false * NULL")]
    [InlineData("true % NULL")]
    [InlineData("false + \"a\"")]
    [InlineData("true - \"a\"")]
    [InlineData("false / \"a\"")]
    [InlineData("true * \"a\"")]
    [InlineData("false % \"a\"")]
    public void Evaluate_InvalidBooleanBinaryOperations_ThrowUnsupportedBinaryOperationException(string expression)
    {
        Assert.Throws<UnsupportedBinaryOperationException>(() =>
        {
            var result = RazeScript.Evaluate(expression);
        });
    }
}
