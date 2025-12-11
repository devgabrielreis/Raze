using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;

namespace Raze.Tests.Core.Variables;

public class StringVariableTests
{
    [Theory]
    [InlineData("var string test = \"teste\"", "test", "teste")]
    [InlineData("var string? test = null", "test", null)]
    public void Evaluate_StringVariableDeclaration_ReturnsExpectedValue(string expression, string varname, string? expected)
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
            Assert.IsType<StringValue>(result);
            Assert.Equal(expected, (result as StringValue)!.StrValue);
        }
    }

    [Theory]
    [InlineData("var string test = true")]
    [InlineData("const string test = 10")]
    [InlineData("var string test = 10.0")]
    [InlineData("var string test = null")]
    public void Evaluate_WrongStringVariableTypeAssignment_ThrowsVariableTypeException(string expression)
    {
        Assert.Throws<VariableTypeException>(() =>
        {
            var result = RazeScript.Evaluate(expression, "Raze.Tests");
        });
    }
}
