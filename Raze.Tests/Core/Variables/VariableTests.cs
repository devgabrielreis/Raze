using Raze.Script.Core;
using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;

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
}
