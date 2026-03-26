using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;

namespace Raze.Tests.Core.Variables;

public class IntegerVariableTests
{
    [Theory]
    [InlineData("const integer test = 5", "test", 5)]
    [InlineData("var integer? test = null", "test", null)]
    public void Evaluate_IntegerVariableDeclaration_ReturnsExpectedValue(string expression, string varname, int? expected)
    {
        var scope = RazeScript.CreateInterpreterScope();
        RazeAssert.EvaluatesToVoid(expression, scope);

        if (expected is null)
        {
            RazeAssert.EvaluatesToNull(varname, scope);
        }
        else
        {
            RazeAssert.EvaluatesToInteger(varname, expected.Value, scope);
        }
    }

    [Theory]
    [InlineData("var integer test = true")]
    [InlineData("var integer test = 10.0")]
    [InlineData("const integer test = \"a\"")]
    [InlineData("const integer test = null")]
    public void Evaluate_WrongIntegerVariableTypeAssignment_ThrowsVariableTypeException(string expression)
    {
        RazeAssert.ReturnsError<VariableTypeException>(expression);
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

        var scope = RazeScript.CreateInterpreterScope();

        RazeAssert.EvaluatesToInteger(script, expressionResult, scope);
        RazeAssert.EvaluatesToInteger("test", variableAfterValue, scope);
    }
}
