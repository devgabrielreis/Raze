using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Runtime.Scopes;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Tests.Core.Types;

public class BooleanTests
{
    [Theory]
    [InlineData("true == true", true)]
    [InlineData("true == false", false)]
    [InlineData("false == true", false)]
    [InlineData("false == false", true)]
    [InlineData("true != true", false)]
    [InlineData("true != false", true)]
    [InlineData("false != true", true)]
    [InlineData("false != false", false)]
    [InlineData("true && true", true)]
    [InlineData("true && false", false)]
    [InlineData("false && true", false)]
    [InlineData("false && false", false)]
    [InlineData("true || true", true)]
    [InlineData("true || false", true)]
    [InlineData("false || true", true)]
    [InlineData("false || false", false)]
    public void Evaluate_BooleanComparisonExpression_ReturnsExpectedValue(string expression, bool expected)
    {
        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(expression, "Raze.Tests", scope);

        Assert.IsType<BooleanValue>(result);
        Assert.Equal(expected, (result as BooleanValue)!.BoolValue);
    }

    [Theory]
    [InlineData("true + 10")]
    [InlineData("false - 10")]
    [InlineData("true / 10")]
    [InlineData("false * 10")]
    [InlineData("true % 10")]
    [InlineData("true == 10")]
    [InlineData("true != 10")]
    [InlineData("true > 10")]
    [InlineData("true < 10")]
    [InlineData("true >= 10")]
    [InlineData("true <= 10")]
    [InlineData("true && 10")]
    [InlineData("true || 10")]
    [InlineData("false + 10.0")]
    [InlineData("true - 10.0")]
    [InlineData("false / 10.0")]
    [InlineData("true * 10.0")]
    [InlineData("false % 10.0")]
    [InlineData("false == 10.0")]
    [InlineData("false != 10.0")]
    [InlineData("false > 10.0")]
    [InlineData("false < 10.0")]
    [InlineData("false >= 10.0")]
    [InlineData("false <= 10.0")]
    [InlineData("false && 10.0")]
    [InlineData("false || 10.0")]
    [InlineData("true + null")]
    [InlineData("false - null")]
    [InlineData("true / null")]
    [InlineData("false * null")]
    [InlineData("true % null")]
    [InlineData("true == null")]
    [InlineData("true != null")]
    [InlineData("true > null")]
    [InlineData("true < null")]
    [InlineData("true >= null")]
    [InlineData("true <= null")]
    [InlineData("true && null")]
    [InlineData("true || null")]
    [InlineData("false + \"a\"")]
    [InlineData("true - \"a\"")]
    [InlineData("false / \"a\"")]
    [InlineData("true * \"a\"")]
    [InlineData("false % \"a\"")]
    [InlineData("false == \"a\"")]
    [InlineData("false != \"a\"")]
    [InlineData("false > \"a\"")]
    [InlineData("false < \"a\"")]
    [InlineData("false >= \"a\"")]
    [InlineData("false <= \"a\"")]
    [InlineData("false && \"a\"")]
    [InlineData("false || \"a\"")]
    [InlineData("true + true")]
    [InlineData("false - false")]
    [InlineData("true / true")]
    [InlineData("false * false")]
    [InlineData("true % true")]
    [InlineData("true > true")]
    [InlineData("true < true")]
    [InlineData("true >= true")]
    [InlineData("true <= true")]
    public void Evaluate_InvalidBooleanBinaryOperations_ThrowUnsupportedBinaryOperationException(string expression)
    {
        Assert.Throws<UnsupportedBinaryOperationException>(() =>
        {
            var result = RazeScript.Evaluate(expression, "Raze.Tests");
        });
    }

    [Theory]
    [InlineData("!true", false)]
    [InlineData("!false", true)]
    public void Evaluate_BooleanUnaryOperation_ReturnsExpectedValue(string expression, bool expected)
    {
        var scope = new InterpreterScope();
        var result = RazeScript.Evaluate(expression, "Raze.Tests", scope);

        Assert.IsType<BooleanValue>(result);
        Assert.Equal(expected, (result as BooleanValue)!.BoolValue);
    }

    [Theory]
    [InlineData("true++")]
    [InlineData("true--")]
    [InlineData("-true")]
    [InlineData("+true")]
    public void Evaluate_InvalidBooleanUnaryOperations_ThrowUnsupportedUnaryOperationException(string expression)
    {
        Assert.Throws<UnsupportedUnaryOperationException>(() =>
        {
            var result = RazeScript.Evaluate(expression, "Raze.Tests");
        });
    }
}
