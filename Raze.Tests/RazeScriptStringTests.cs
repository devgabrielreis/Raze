using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;
using Raze.Script.Core;

namespace Raze.Tests;

public class RazeScriptStringTests
{
    [Theory]
    [InlineData("var string test = \"teste\"", "test", "teste")]
    [InlineData("var string? test = null", "test", null)]
    public void Evaluate_StringVariableDeclaration_ReturnsExpectedValue(string expression, string varname, string? expected)
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate(expression, scope);

        var result = RazeScript.Evaluate(varname, scope);

        Assert.IsType<StringValue>(result);

        Assert.Equal(expected, (result as StringValue)!.StrValue);
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
            var result = RazeScript.Evaluate(expression);
        });
    }

    [Fact]
    public void Evaluate_StringConcatenationExpression_ReturnsExpectedValue()
    {
        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate("\"Hello\" + \" \" + \"World!\"", scope);

        Assert.IsType<StringValue>(result);
        Assert.Equal("Hello World!", (result as StringValue)!.StrValue);
    }

    [Theory]
    [InlineData("\"aaa\" == \"aaa\"", true)]
    [InlineData("\"aaa\" == \"bbb\"", false)]
    [InlineData("\"aaa\" == \"AAA\"", false)]
    [InlineData("\"\" == \"\"", true)]
    [InlineData("\"aaa\" != \"aaa\"", false)]
    [InlineData("\"aaa\" != \"bbb\"", true)]
    [InlineData("\"aaa\" != \"AAA\"", true)]
    [InlineData("\"\" != \"\"", false)]
    public void Evaluate_StringComparisonExpression_ReturnsExpectedValue(string expression, bool expected)
    {
        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(expression, scope);

        Assert.IsType<BooleanValue>(result);
        Assert.Equal(expected, (result as BooleanValue)!.BoolValue);
    }

    [Theory]
    [InlineData("\"a\" + 10")]
    [InlineData("\"a\" - 10")]
    [InlineData("\"a\" / 10")]
    [InlineData("\"a\" * 10")]
    [InlineData("\"a\" % 10")]
    [InlineData("\"a\" == 10")]
    [InlineData("\"a\" != 10")]
    [InlineData("\"a\" > 10")]
    [InlineData("\"a\" < 10")]
    [InlineData("\"a\" >= 10")]
    [InlineData("\"a\" <= 10")]
    [InlineData("\"a\" && 10")]
    [InlineData("\"a\" || 10")]
    [InlineData("\"a\" + 10.0")]
    [InlineData("\"a\" - 10.0")]
    [InlineData("\"a\" / 10.0")]
    [InlineData("\"a\" * 10.0")]
    [InlineData("\"a\" % 10.0")]
    [InlineData("\"a\" == 10.0")]
    [InlineData("\"a\" != 10.0")]
    [InlineData("\"a\" > 10.0")]
    [InlineData("\"a\" < 10.0")]
    [InlineData("\"a\" >= 10.0")]
    [InlineData("\"a\" <= 10.0")]
    [InlineData("\"a\" && 10.0")]
    [InlineData("\"a\" || 10.0")]
    [InlineData("\"a\" + true")]
    [InlineData("\"a\" - true")]
    [InlineData("\"a\" / true")]
    [InlineData("\"a\" * true")]
    [InlineData("\"a\" % true")]
    [InlineData("\"a\" == true")]
    [InlineData("\"a\" != true")]
    [InlineData("\"a\" > true")]
    [InlineData("\"a\" < true")]
    [InlineData("\"a\" >= true")]
    [InlineData("\"a\" <= true")]
    [InlineData("\"a\" && true")]
    [InlineData("\"a\" || true")]
    [InlineData("\"a\" + null")]
    [InlineData("\"a\" - null")]
    [InlineData("\"a\" / null")]
    [InlineData("\"a\" * null")]
    [InlineData("\"a\" % null")]
    [InlineData("\"a\" == null")]
    [InlineData("\"a\" != null")]
    [InlineData("\"a\" > null")]
    [InlineData("\"a\" < null")]
    [InlineData("\"a\" >= null")]
    [InlineData("\"a\" <= null")]
    [InlineData("\"a\" && null")]
    [InlineData("\"a\" || null")]
    [InlineData("\"a\" - \"a\"")]
    [InlineData("\"a\" / \"a\"")]
    [InlineData("\"a\" * \"a\"")]
    [InlineData("\"a\" % \"a\"")]
    [InlineData("\"a\" > \"a\"")]
    [InlineData("\"a\" < \"a\"")]
    [InlineData("\"a\" >= \"a\"")]
    [InlineData("\"a\" <= \"a\"")]
    [InlineData("\"a\" && \"a\"")]
    [InlineData("\"a\" || \"a\"")]
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
        Assert.Equal(expected.ToString(), (result as StringValue)!.StrValue);
    }

    [Theory]
    [InlineData("+")]
    [InlineData("==")]
    [InlineData("!=")]
    public void Evaluate_StringOperationWithNullStringVariable_ThrowsNullValueException(string op)
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate("var string? nullVar = null", scope);

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate($"\"a\" {op} nullVar", scope);
        });

        Assert.Throws<NullValueException>(() =>
        {
            RazeScript.Evaluate($"nullVar {op} \"a\"", scope);
        });
    }
}
