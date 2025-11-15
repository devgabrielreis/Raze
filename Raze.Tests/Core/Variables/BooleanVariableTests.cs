using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;

namespace Raze.Tests.Core.Variables;

public class BooleanVariableTests
{
    [Theory]
    [InlineData("var boolean test = true", "test", true)]
    [InlineData("const boolean test = false", "test", false)]
    [InlineData("var boolean? test = null", "test", null)]
    public void Evaluate_BooleanVariableDeclaration_ReturnsExpectedValue(string expression, string varname, bool? expected)
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate(expression, scope);

        var result = RazeScript.Evaluate(varname, scope);

        Assert.IsType<BooleanValue>(result);

        Assert.Equal(expected, (result as BooleanValue)!.BoolValue);
    }

    [Theory]
    [InlineData("var boolean test = 10")]
    [InlineData("var boolean test = 10.0")]
    [InlineData("const boolean test = \"a\"")]
    [InlineData("const boolean test = null")]
    public void Evaluate_WrongBooleanVariableTypeAssignment_ThrowsVariableTypeException(string expression)
    {
        Assert.Throws<VariableTypeException>(() =>
        {
            var result = RazeScript.Evaluate(expression);
        });
    }

    [Theory]
    [InlineData("==")]
    [InlineData("!=")]
    [InlineData("&&")]
    [InlineData("||")]
    public void Evaluate_BooleanOperationWithNullBooleanVariable_ThrowsNullValueException(string op)
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate("var boolean? nullVar = null", scope);

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate($"false {op} nullVar", scope);
        });

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate($"nullVar {op} true", scope);
        });
    }
}
