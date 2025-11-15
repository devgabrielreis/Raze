using Raze.Script.Core;
using Raze.Script.Core.Exceptions.LexerExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;

namespace Raze.Tests.Core.Types;

public class StringTests
{
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
    [InlineData("\"hello\\zword\"")]
    [InlineData("\"\\'\"")]
    public void Evaluate_InvalidEscapeSequence_ThrowsUnrecognizedEscapeSequenceException(string expression)
    {
        Assert.Throws<UnrecognizedEscapeSequenceException>(() =>
        {
            RazeScript.Evaluate(expression);
        });
    }
}
