using Raze.Script.Core.Exceptions.ParseExceptions;

namespace Raze.Tests.Core.Types;

public class VoidTests
{
    [Fact]
    public void Evaluate_DeclaringVoidParameter_ThrowsInvalidSymbolDeclarationException()
    {
        var script = @"
            def void myFunc(void param) {
                return;
            }

            myFunc();
        ";

        RazeAssert.ReturnsError<InvalidSymbolDeclarationException>(script);
    }

    [Fact]
    public void Evaluate_DeclaringVoidVariable_ThrowsInvalidSymbolDeclarationException()
    {
        var script = @"
            var void test
        ";

        RazeAssert.ReturnsError<InvalidSymbolDeclarationException>(script);
    }

    [Fact]
    public void Evaluate_NullableVoidType_ThrowsInvalidTypeDeclarationException()
    {
        var script = @"
            def void? myFunction() {
                return null;
            }
        ";

        RazeAssert.ReturnsError<InvalidTypeDeclarationException>(script);
    }
}
