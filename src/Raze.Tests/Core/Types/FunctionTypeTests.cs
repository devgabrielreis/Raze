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

        RazeAssert.ReturnsError<VariableTypeException>(script);
    }

    [Fact]
    public void Evaluate_FunctionTypeWithoutGenerics_ThrowsInvalidTypeDeclarationException()
    {
        RazeAssert.ReturnsError<InvalidTypeDeclarationException>("var function myVar");
        RazeAssert.ReturnsError<InvalidTypeDeclarationException>("var function<> myVar");
    }
}
