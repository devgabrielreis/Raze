using Raze.Script.Core.Scopes;
using Raze.Script.Core;
using Raze.Script.Core.Values;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using System.Globalization;

namespace Raze.Tests;

public class RazeScriptBooleanTests
{
    [Theory]
    [InlineData("var boolean test = true", "test", true)]
    [InlineData("const boolean test = false", "test", false)]
    [InlineData("var boolean test = null", "test", null)]
    [InlineData("var boolean test", "test", null)]
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
    public void Evaluate_WrongBooleanVariableTypeAssignment_ThrowsVariableTypeException(string expression)
    {
        Assert.Throws<VariableTypeException>(() =>
        {
            var result = RazeScript.Evaluate(expression);
        });
    }

    [Theory]
    [InlineData("true == true", true)]
    [InlineData("true == false", false)]
    [InlineData("false == true", false)]
    [InlineData("false == false", true)]
    [InlineData("true != true", false)]
    [InlineData("true != false", true)]
    [InlineData("false != true", true)]
    [InlineData("false != false", false)]
    public void Evaluate_BooleanComparisonExpression_ReturnsExpectedValue(string expression, bool expected)
    {
        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(expression, scope);

        Assert.IsType<BooleanValue>(result);
        Assert.Equal(expected, (result as BooleanValue)!.BoolValue);
    }

    [Theory]
    [InlineData("true + 10")]
    [InlineData("false - 10")]
    [InlineData("true / 10")]
    [InlineData("false * 10")]
    [InlineData("true % 10")]
    [InlineData("true == 10")]
    [InlineData("true != 10")]
    [InlineData("true > 10")]
    [InlineData("true < 10")]
    [InlineData("true >= 10")]
    [InlineData("false + 10.0")]
    [InlineData("true - 10.0")]
    [InlineData("false / 10.0")]
    [InlineData("true * 10.0")]
    [InlineData("false % 10.0")]
    [InlineData("false == 10.0")]
    [InlineData("false != 10.0")]
    [InlineData("false > 10.0")]
    [InlineData("false < 10.0")]
    [InlineData("false >= 10.0")]
    [InlineData("true + null")]
    [InlineData("false - null")]
    [InlineData("true / null")]
    [InlineData("false * null")]
    [InlineData("true % null")]
    [InlineData("true == null")]
    [InlineData("true != null")]
    [InlineData("true > null")]
    [InlineData("true < null")]
    [InlineData("true >= null")]
    [InlineData("false + \"a\"")]
    [InlineData("true - \"a\"")]
    [InlineData("false / \"a\"")]
    [InlineData("true * \"a\"")]
    [InlineData("false % \"a\"")]
    [InlineData("false == \"a\"")]
    [InlineData("false != \"a\"")]
    [InlineData("false > \"a\"")]
    [InlineData("false < \"a\"")]
    [InlineData("false >= \"a\"")]
    [InlineData("true + true")]
    [InlineData("false - false")]
    [InlineData("true / true")]
    [InlineData("false * false")]
    [InlineData("true % true")]
    [InlineData("true > true")]
    [InlineData("true < true")]
    [InlineData("true >= true")]
    public void Evaluate_InvalidBooleanBinaryOperations_ThrowUnsupportedBinaryOperationException(string expression)
    {
        Assert.Throws<UnsupportedBinaryOperationException>(() =>
        {
            var result = RazeScript.Evaluate(expression);
        });
    }

    [Theory]
    [InlineData("==")]
    [InlineData("!=")]
    public void Evaluate_BooleanOperationWithNullBooleanVariable_ThrowsNullValueException(string op)
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate("var boolean nullVar", scope);

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
