using Raze.Script.Core;
using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;

namespace Raze.Tests.Core.Variables;

public class VariableTests
{
    [Fact]
    public void Evaluate_VariableAssignment_CreatesCopy()
    {
        var scope = RazeScript.CreateInterpreterScope();

        RazeAssert.EvaluatesToVoid("var integer a = 10", scope);
        RazeAssert.EvaluatesToVoid("var integer b = a", scope);
        RazeAssert.EvaluatesToVoid("a = 20", scope);

        RazeAssert.EvaluatesToInteger("b", 10, scope);
    }

    [Fact]
    public void Evaluate_ConstantAssignment_ThrowsConstantAssignmentException()
    {
        var scope = RazeScript.CreateInterpreterScope();

        RazeAssert.EvaluatesToVoid("const integer test = 10", scope);
        RazeAssert.ReturnsError<ConstantAssignmentException>("test = 11", scope);
    }

    [Fact]
    public void Evaluate_AccessingUninitializedVariable_ThrowsUninitializedVariableException()
    {
        var scope = RazeScript.CreateInterpreterScope();

        RazeAssert.EvaluatesToVoid("var integer variable", scope);
        RazeAssert.EvaluatesToVoid("var integer? nullableVariable", scope);

        RazeAssert.ReturnsError<UninitializedVariableException>("variable", scope);
        RazeAssert.ReturnsError<UninitializedVariableException>("nullableVariable", scope);
    }

    [Fact]
    public void Evaluate_UninitializedVariable_CanBeAssignedTo()
    {
        var script = $@"
            var integer a;
            a = 80;
            a
        ";

        RazeAssert.EvaluatesToInteger(script, 80);
    }

    [Fact]
    public void Evaluate_UninitializedConstant_ThrowsUninitializedConstantException()
    {
        RazeAssert.ReturnsError<UninitializedConstantException>("const integer constant");
        RazeAssert.ReturnsError<UninitializedConstantException>("const integer? nullableConstant");
    }

    [Fact]
    public void Evaluate_AssigningNullToNonNullableVariable_ThrowsVariableTypeException()
    {
        RazeAssert.ReturnsError<VariableTypeException>("var integer variable = null");
    }

    [Fact]
    public void Evaluate_DeclaringVoidTypeVariable_ThrowsInvalidSymbolDeclarationException()
    {
        RazeAssert.ReturnsError<InvalidSymbolDeclarationException>("var void variable");
    }

    [Fact]
    public void Evaluate_NullCheckerOperator_ReturnsExpectedValue()
    {
        var script = @"
            var decimal? my_var = 10.0;
            my_var??
        ";
        RazeAssert.EvaluatesToBoolean(script, false);

        script = @"
            var integer? my_var = null;
            my_var??
        ";
        RazeAssert.EvaluatesToBoolean(script, true);

        script = @"
            var boolean my_var = true;
            my_var??
        ";
        RazeAssert.EvaluatesToBoolean(script, false);
    }

    [Theory]
    [InlineData(10, "+=", 10, 20)]
    [InlineData(10, "-=", 10, 0)]
    [InlineData(10, "*=", 10, 100)]
    [InlineData(10, "/=", 2, 5)]
    [InlineData(10, "%=", 3, 1)]
    public void Evaluate_CompoundAssignmentOperation_ExecutesCorrespondingOperation(
        int startValue, string op, int rightSideValue, int expectedResult
    )
    {
        var script = $@"
            var integer test = {startValue};
            test {op} {rightSideValue};
            test;
        ";

        RazeAssert.EvaluatesToInteger(script, expectedResult);
    }

    [Theory]
    [InlineData("+=")]
    [InlineData("-=")]
    [InlineData("*=")]
    [InlineData("/=")]
    [InlineData("%=")]
    public void Evaluate_DeclaringVaribleWithCompoundAssignmentOperator_ThrowsUnexpectedTokenException(string op)
    {
        var script = $"""
            var integer test {op} 10
        """;

        RazeAssert.ReturnsError<UnexpectedTokenException>(script);
    }

    [Theory]
    [InlineData("+=")]
    [InlineData("-=")]
    [InlineData("*=")]
    [InlineData("/=")]
    [InlineData("%=")]
    public void Evaluate_DeclaringConstantWithCompoundAssignmentOperator_ThrowsUnexpectedTokenException(string op)
    {
        var script = $"""
            const integer test {op} 10
        """;

        RazeAssert.ReturnsError<UnexpectedTokenException>(script);
    }
}
