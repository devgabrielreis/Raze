using Raze.Script.Core.Exceptions.LexerExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;

namespace Raze.Tests.Core.Types;

public class StringTests
{
    [Fact]
    public void Evaluate_StringConcatenationExpression_ReturnsExpectedValue()
    {
        RazeAssert.EvaluatesToString("\"Hello\" + \" \" + \"World!\"", "Hello World!");
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
        RazeAssert.EvaluatesToBoolean(expression, expected);
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
        RazeAssert.ReturnsError<UnsupportedBinaryOperationException>(expression);
    }

    [Theory]
    [InlineData("\"\\\"\"", '"')]
    [InlineData("\"\\n\"", '\n')]
    [InlineData("\"\\t\"", '\t')]
    [InlineData("\"\\r\"", '\r')]
    [InlineData("\"\\\\\"", '\\')]
    public void Evaluate_EscapeCharacter_ReturnsExpectedValue(string character, char expected)
    {
        RazeAssert.EvaluatesToString(character, expected.ToString());
    }

    [Theory]
    [InlineData("\"hello\\zword\"")]
    [InlineData("\"\\'\"")]
    public void Evaluate_InvalidEscapeSequence_ThrowsUnrecognizedEscapeSequenceException(string expression)
    {
        RazeAssert.ReturnsError<UnrecognizedEscapeSequenceException>(expression);
    }

    [Theory]
    [InlineData("-\"teste\"")]
    [InlineData("+\"teste\"")]
    [InlineData("!\"teste\"")]
    public void Evaluate_InvalidStringUnaryOperations_ThrowUnsupportedUnaryOperationException(string expression)
    {
        RazeAssert.ReturnsError<UnsupportedUnaryOperationException>(expression);
    }
}
