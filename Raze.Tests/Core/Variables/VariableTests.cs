using Raze.Script.Core;
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

        RazeScript.Evaluate("var integer a = 10", scope);
        RazeScript.Evaluate("var integer b = a", scope);
        RazeScript.Evaluate("a = 20", scope);
        var result = RazeScript.Evaluate("b", scope);

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(10, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_ConstantAssignment_ThrowsConstantAssignmentException()
    {
        var scope = new InterpreterScope();

        RazeScript.Evaluate("const integer test = 10", scope);

        Assert.Throws<ConstantAssignmentException>(() =>
        {
            RazeScript.Evaluate("test = 11", scope);
        });
    }

    [Fact]
    public void Evaluate_UninitializedVariable_ThrowsUninitializedVariableException()
    {
        var scope = new InterpreterScope();

        RazeScript.Evaluate("var integer variable", scope);
        RazeScript.Evaluate("var integer? nullableVariable", scope);

        Assert.Throws<UninitializedVariableException>(() =>
        {
            RazeScript.Evaluate("variable", scope);
        });

        Assert.Throws<UninitializedVariableException>(() =>
        {
            RazeScript.Evaluate("nullableVariable", scope);
        });
    }

    [Fact]
    public void Evaluate_AssigningNullToNonNullableVariable_ThrowsVariableTypeException()
    {
        Assert.Throws<VariableTypeException>(() =>
        {
            RazeScript.Evaluate("var integer variable = null");
        });
    }
}
