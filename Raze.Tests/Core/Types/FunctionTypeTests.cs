using Raze.Script.Core;
using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;

namespace Raze.Tests.Core.Types;

public class FunctionTypeTests
{
    [Fact]
    public void Evaluate_FunctionVariableTypeNotMatchingAssignedValue_ThrowsVariableTypeException()
    {
        var script = @"
            def integer myFunc(integer param) {
                return 10;
            }

            var function<integer, decimal> myVar = myFunc;
        ";

        Assert.Throws<VariableTypeException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_FunctionTypeWithoutGenerics_ThrowsUnexpectedTokenException()
    {
        var script = @"
            var function myVar;
        ";

        Assert.Throws<UnexpectedTokenException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
    }
}
