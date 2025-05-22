using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;

namespace Raze.Tests;

public class RazeScriptScopeTests
{
    [Fact]
    public void Evaluate_DuplicateVariableDeclaration_ThrowsException()
    {
        var scope = new InterpreterScope();
        RazeScript.Evaluate("var integer x = 1", scope);

        Assert.Throws<RedeclarationException>(() =>
        {
            RazeScript.Evaluate("var integer x = 2", scope);
        });

        var scope2 = new InterpreterScope();
        RazeScript.Evaluate("var integer x = 1", scope2);

        Assert.Throws<RedeclarationException>(() =>
        {
            RazeScript.Evaluate("const integer x = 2", scope2);
        });
    }
}
