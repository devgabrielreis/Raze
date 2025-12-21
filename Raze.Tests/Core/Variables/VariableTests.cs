using Raze.Script.Core;
using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Runtime.Scopes;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Tests.Core.Variables;

public class VariableTests
{
    [Fact]
    public void Evaluate_VariableAssignment_CreatesCopy()
    {
        var scope = new InterpreterScope();

        RazeScript.Evaluate("var integer a = 10", "Raze.Tests", scope);
        RazeScript.Evaluate("var integer b = a", "Raze.Tests", scope);
        RazeScript.Evaluate("a = 20", "Raze.Tests", scope);
        var result = RazeScript.Evaluate("b", "Raze.Tests", scope);

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(10, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_ConstantAssignment_ThrowsConstantAssignmentException()
    {
        var scope = new InterpreterScope();

        RazeScript.Evaluate("const integer test = 10", "Raze.Tests", scope);

        Assert.Throws<ConstantAssignmentException>(() =>
        {
            RazeScript.Evaluate("test = 11", "Raze.Tests", scope);
        });
    }

    [Fact]
    public void Evaluate_UninitializedVariable_ThrowsUninitializedVariableException()
    {
        var scope = new InterpreterScope();

        RazeScript.Evaluate("var integer variable", "Raze.Tests", scope);
        RazeScript.Evaluate("var integer? nullableVariable", "Raze.Tests", scope);

        Assert.Throws<UninitializedVariableException>(() =>
        {
            RazeScript.Evaluate("variable", "Raze.Tests", scope);
        });

        Assert.Throws<UninitializedVariableException>(() =>
        {
            RazeScript.Evaluate("nullableVariable", "Raze.Tests", scope);
        });
    }

    [Fact]
    public void Evaluate_UninitializedConstant_ThrowsUninitializedConstantException()
    {
        Assert.Throws<UninitializedConstantException>(() =>
        {
            RazeScript.Evaluate("const integer constant", "Raze.Tests");
        });

        Assert.Throws<UninitializedConstantException>(() =>
        {
            RazeScript.Evaluate("const integer? nullableConstant", "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_AssigningNullToNonNullableVariable_ThrowsVariableTypeException()
    {
        Assert.Throws<VariableTypeException>(() =>
        {
            RazeScript.Evaluate("var integer variable = null", "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_DeclaringVoidTypeVariable_ThrowsInvalidSymbolDeclarationException()
    {
        Assert.Throws<InvalidSymbolDeclarationException>(() =>
        {
            RazeScript.Evaluate("var void variable", "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_NullCheckerOperator_ReturnsExpectedValue()
    {
        var script = @"
            var decimal? my_var = 10.0;
            my_var??
        ";

        var result = RazeScript.Evaluate(script, "Raze.Tests");
        Assert.IsType<BooleanValue>(result);
        Assert.False(((BooleanValue)result).BoolValue);

        script = @"
            var integer? my_var = null;
            my_var??
        ";

        result = RazeScript.Evaluate(script, "Raze.Tests");
        Assert.IsType<BooleanValue>(result);
        Assert.True(((BooleanValue)result).BoolValue);

        script = @"
            var boolean my_var = true;
            my_var??
        ";

        result = RazeScript.Evaluate(script, "Raze.Tests");
        Assert.IsType<BooleanValue>(result);
        Assert.False(((BooleanValue)result).BoolValue);
    }

    [Theory]
    [InlineData("integer", "10", "+=", "10", "20")]
    [InlineData("integer", "10", "-=", "10", "0")]
    [InlineData("integer", "10", "*=", "10", "100")]
    [InlineData("integer", "10", "/=", "2", "5")]
    [InlineData("integer", "10", "%=", "3", "1")]
    public void Evaluate_CompoundAssignmentOperation_ExecutesCorrespondingOperation(
        string variableType, string startValueStr, string op, string rightSideValue, string expectedResultStr
    )
    {
        var script = $@"
            var {variableType} test = {startValueStr};
            test {op} {rightSideValue};
            test;
        ";

        var result = RazeScript.Evaluate(script, "Raze.Tests");
        Assert.Equal(expectedResultStr, result.ToString());
    }
}
