using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Runtime.Scopes;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Tests.Core.Variables;

public class IntegerVariableTests
{
    [Theory]
    [InlineData("const integer test = 5", "test", 5)]
    [InlineData("var integer? test = null", "test", null)]
    public void Evaluate_IntegerVariableDeclaration_ReturnsExpectedValue(string expression, string varname, int? expected)
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate(expression, "Raze.Tests", scope);

        var result = RazeScript.Evaluate(varname, "Raze.Tests", scope);

        if (expected is null)
        {
            Assert.IsType<NullValue>(result);
        }
        else
        {
            Assert.IsType<IntegerValue>(result);

            Assert.Equal((Int128)expected!, (result as IntegerValue)!.IntValue);
        }
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
            var result = RazeScript.Evaluate(expression, "Raze.Tests");
        });
    }

    [Theory]
    [InlineData("++", false, 10, 11, 11)]
    [InlineData("++", true, 10, 10, 11)]
    [InlineData("--", false, 10, 9, 9)]
    [InlineData("--", true, 10, 10, 9)]
    public void Evaluate_UnaryMutationOperator_ReturnsExpectedValueAndChangeVariable(
        string op, bool isPostfix, int variableInitualValue, int expressionResult, int variableAfterValue
    )
    {
        var script = $@"
            var integer test = {variableInitualValue};
            {(isPostfix ? $"test{op}" : $"{op}test")}
        ";

        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(script, "Raze.Tests", scope);

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(expressionResult, (result as IntegerValue)!.IntValue);

        result = RazeScript.Evaluate("test", "Raze.Tests", scope);

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(variableAfterValue, (result as IntegerValue)!.IntValue);
    }
}
