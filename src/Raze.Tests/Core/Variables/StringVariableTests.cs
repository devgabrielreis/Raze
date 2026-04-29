using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;

namespace Raze.Tests.Core.Variables;

public class StringVariableTests
{
    [Theory]
    [InlineData("var string test = \"teste\"", "test", "teste")]
    [InlineData("var string? test = null", "test", null)]
    public void Evaluate_StringVariableDeclaration_ReturnsExpectedValue(string expression, string varname, string? expected)
    {
        var session = new RazeSession();
        RazeAssert.EvaluatesToVoid(expression, session);

        if (expected is null)
        {
            RazeAssert.EvaluatesToNull(varname, session);
        }
        else
        {
            RazeAssert.EvaluatesToString(varname, expected, session);
        }
    }

    [Theory]
    [InlineData("var string test = true")]
    [InlineData("const string test = 10")]
    [InlineData("var string test = 10.0")]
    [InlineData("var string test = null")]
    public void Evaluate_WrongStringVariableTypeAssignment_ThrowsVariableTypeException(string expression)
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
        var script = $"""
            var string test = "test";
            {(isPostfix ? $"test{op}" : $"{op}test")}
        """;

        RazeAssert.ReturnsError<UnsupportedUnaryOperationException>(script);
    }
}
