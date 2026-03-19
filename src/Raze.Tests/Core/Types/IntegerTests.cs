using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;

namespace Raze.Tests.Core.Types;

public class IntegerTests
{
    [Theory]
    [InlineData("10 + 10", 20)]
    [InlineData("10 - 15", -5)]
    [InlineData("6 / 2", 3)]
    [InlineData("10 * 3", 30)]
    [InlineData("10 % 3", 1)]
    public void Evaluate_IntegerArithmeticExpression_ReturnsExpectedValue(string expression, int expected)
    {
        RazeAssert.EvaluatesToInteger(expression, expected);
    }

    [Theory]
    [InlineData("10 == 10", true)]
    [InlineData("10 == 11", false)]
    [InlineData("10 != 10", false)]
    [InlineData("10 != 11", true)]
    [InlineData("10 > 9", true)]
    [InlineData("10 > 10", false)]
    [InlineData("10 > 11", false)]
    [InlineData("10 < 9", false)]
    [InlineData("10 < 10", false)]
    [InlineData("10 < 11", true)]
    [InlineData("10 >= 9", true)]
    [InlineData("10 >= 10", true)]
    [InlineData("10 >= 11", false)]
    [InlineData("10 <= 9", false)]
    [InlineData("10 <= 10", true)]
    [InlineData("10 <= 11", true)]
    public void Evaluate_IntegerComparisonExpression_ReturnsExpectedValue(string expression, bool expected)
    {
        RazeAssert.EvaluatesToBoolean(expression, expected);
    }

    [Theory]
    [InlineData("10 + 10.0")]
    [InlineData("10 - 15.0")]
    [InlineData("6 / 2.0")]
    [InlineData("10 * 3.0")]
    [InlineData("10 % 3.0")]
    [InlineData("10 == 10.0")]
    [InlineData("10 != 10.1")]
    [InlineData("10 > 9.9")]
    [InlineData("10 < 9.9")]
    [InlineData("10 >= 9.9")]
    [InlineData("10 <= 9.9")]
    [InlineData("10 && 10.0")]
    [InlineData("10 || 10.0")]
    [InlineData("10 && 10")]
    [InlineData("10 || 10")]
    [InlineData("10 + true")]
    [InlineData("10 - true")]
    [InlineData("10 / true")]
    [InlineData("10 * true")]
    [InlineData("10 % true")]
    [InlineData("10 == true")]
    [InlineData("10 != true")]
    [InlineData("10 > true")]
    [InlineData("10 < true")]
    [InlineData("10 >= true")]
    [InlineData("10 <= true")]
    [InlineData("10 && true")]
    [InlineData("10 || true")]
    [InlineData("10 + null")]
    [InlineData("10 - null")]
    [InlineData("10 / null")]
    [InlineData("10 * null")]
    [InlineData("10 % null")]
    [InlineData("10 == null")]
    [InlineData("10 != null")]
    [InlineData("10 > null")]
    [InlineData("10 < null")]
    [InlineData("10 >= null")]
    [InlineData("10 <= null")]
    [InlineData("10 && null")]
    [InlineData("10 || null")]
    [InlineData("10 + \"a\"")]
    [InlineData("10 - \"a\"")]
    [InlineData("10 / \"a\"")]
    [InlineData("10 * \"a\"")]
    [InlineData("10 % \"a\"")]
    [InlineData("10 == \"a\"")]
    [InlineData("10 != \"a\"")]
    [InlineData("10 > \"a\"")]
    [InlineData("10 < \"a\"")]
    [InlineData("10 >= \"a\"")]
    [InlineData("10 <= \"a\"")]
    [InlineData("10 && \"a\"")]
    [InlineData("10 || \"a\"")]
    public void Evaluate_InvalidIntegerBinaryOperations_ThrowUnsupportedBinaryOperationException(string expression)
    {
        RazeAssert.ReturnsError<UnsupportedBinaryOperationException>(expression);
    }

    [Theory]
    [InlineData("+10", 10)]
    [InlineData("-10", -10)]
    public void Evaluate_IntegerUnaryOperation_ReturnsExpectedValue(string expression, int expected)
    {
        RazeAssert.EvaluatesToInteger(expression, expected);
    }

    [Theory]
    [InlineData("!10")]
    public void Evaluate_InvalidIntegerUnaryOperations_ThrowUnsupportedUnaryOperationException(string expression)
    {
        RazeAssert.ReturnsError<UnsupportedUnaryOperationException>(expression);
    }

    [Fact]
    public void Evaluate_IntegerTypeWithGenerics_ThrowInvalidTypeDeclarationException()
    {
        RazeAssert.ReturnsError<InvalidTypeDeclarationException>("var integer<integer> myVar");
        RazeAssert.ReturnsError<InvalidTypeDeclarationException>("var integer<> myVar");
    }
}
