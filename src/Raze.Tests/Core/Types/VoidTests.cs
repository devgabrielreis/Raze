using Raze.Script.Core;
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

        Assert.Throws<InvalidSymbolDeclarationException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_DeclaringVoidVariable_ThrowsInvalidSymbolDeclarationException()
    {
        var script = @"
            var void test
        ";

        Assert.Throws<InvalidSymbolDeclarationException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_NullableVoidType_ThrowsInvalidTypeDeclarationException()
    {
        var script = @"
            def void? myFunction() {
                return null;
            }
        ";

        Assert.Throws<InvalidTypeDeclarationException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
    }
}
