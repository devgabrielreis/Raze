using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;

namespace Raze.Tests.Core.Variables;

public class BooleanVariableTests
{
    [Theory]
    [InlineData("var boolean test = true", "test", true)]
    [InlineData("const boolean test = false", "test", false)]
    [InlineData("var boolean? test = null", "test", null)]
    public void Evaluate_BooleanVariableDeclaration_ReturnsExpectedValue(string expression, string varname, bool? expected)
    {
        var scope = RazeScript.CreateInterpreterScope();
        RazeAssert.EvaluatesToVoid(expression, scope);

        if (expected is null)
        {
            RazeAssert.EvaluatesToNull(varname, scope);
        }
        else
        {
            RazeAssert.EvaluatesToBoolean(varname, expected.Value, scope);
        }
    }

    [Theory]
    [InlineData("var boolean test = 10")]
    [InlineData("var boolean test = 10.0")]
    [InlineData("const boolean test = \"a\"")]
    [InlineData("const boolean test = null")]
    public void Evaluate_WrongBooleanVariableTypeAssignment_ThrowsVariableTypeException(string expression)
    {
        RazeAssert.ReturnsError<VariableTypeException>(expression);
    }

    [Theory]
    [InlineData("++", false)]
    [InlineData("--", false)]
    [InlineData("++", true)]
    [InlineData("--", true)]
    public void Evaluate_UnsupportedUnaryMutationExpression_ThrowsUnsupportedUnaryOperationException(string op, bool isPostfix)
    {
        var script = $@"
            var boolean test = false;
            {(isPostfix ? $"test{op}" : $"{op}test")}
        ";

        RazeAssert.ReturnsError<UnsupportedUnaryOperationException>(script);
    }
}
