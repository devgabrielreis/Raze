using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;
using Raze.Script.Core;

namespace Raze.Tests;

public class RazeScriptStringTests
{
    [Theory]
    [InlineData("var string test = \"teste\"", "test", "teste")]
    [InlineData("var string test = NULL", "test", null)]
    [InlineData("const string test", "test", null)]
    public void Evaluate_StringVariableDeclaration_ReturnsExpectedValue(string expression, string varname, string? expected)
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate(expression, scope);

        var result = RazeScript.Evaluate(varname, scope);

        Assert.IsType<StringValue>(result);

        Assert.Equal(expected, (string?)((result as StringValue)!.Value)!);
    }

    [Theory]
    [InlineData("var string test = true")]
    [InlineData("const string test = 10")]
    [InlineData("var string test = 10.0")]
    public void Evaluate_WrongStringVariableTypeAssignment_ThrowsVariableTypeException(string expression)
    {
        Assert.Throws<VariableTypeException>(() =>
        {
            var result = RazeScript.Evaluate(expression);
        });
    }

    [Fact]
    public void Evaluate_StringConcatenationExpression_ReturnsExpectedValue()
    {
        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate("\"Hello\" + \" \" + \"World!\"", scope);

        Assert.IsType<StringValue>(result);
        Assert.Equal("Hello World!", (string)((result as StringValue)!.Value)!);
    }

    [Theory]
    [InlineData("\"a\" + 10")]
    [InlineData("\"a\" - 10")]
    [InlineData("\"a\" / 10")]
    [InlineData("\"a\" * 10")]
    [InlineData("\"a\" % 10")]
    [InlineData("\"a\" + 10.0")]
    [InlineData("\"a\" - 10.0")]
    [InlineData("\"a\" / 10.0")]
    [InlineData("\"a\" * 10.0")]
    [InlineData("\"a\" % 10.0")]
    [InlineData("\"a\" + true")]
    [InlineData("\"a\" - true")]
    [InlineData("\"a\" / true")]
    [InlineData("\"a\" * true")]
    [InlineData("\"a\" % true")]
    [InlineData("\"a\" + NULL")]
    [InlineData("\"a\" - NULL")]
    [InlineData("\"a\" / NULL")]
    [InlineData("\"a\" * NULL")]
    [InlineData("\"a\" % NULL")]
    [InlineData("\"a\" - \"a\"")]
    [InlineData("\"a\" / \"a\"")]
    [InlineData("\"a\" * \"a\"")]
    [InlineData("\"a\" % \"a\"")]
    public void Evaluate_InvalidStringBinaryOperations_ThrowUnsupportedBinaryOperationException(string expression)
    {
        Assert.Throws<UnsupportedBinaryOperationException>(() =>
        {
            var result = RazeScript.Evaluate(expression);
        });
    }

    [Theory]
    [InlineData("\"\\\"\"", '"')]
    [InlineData("\"\\n\"", '\n')]
    [InlineData("\"\\t\"", '\t')]
    [InlineData("\"\\r\"", '\r')]
    [InlineData("\"\\\\\"", '\\')]
    public void Evaluate_EscapeCharacter_ReturnsExpectedValue(string character, char expected)
    {
        var result = RazeScript.Evaluate(character);

        Assert.IsType<StringValue>(result);
        Assert.Equal(expected.ToString(), (string)result.Value!);
    }

    [Theory]
    [InlineData("+")]
    public void Evaluate_StringOperationWithNullStringVariable_ThrowsNullValueException(string op)
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate("var string nullVar", scope);

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate($"\"a\" {op} nullVar", scope);
        });
    }
}
