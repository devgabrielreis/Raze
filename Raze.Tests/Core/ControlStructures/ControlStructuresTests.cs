using Raze.Script.Core;
using Raze.Script.Core.Exceptions.RuntimeExceptions;

namespace Raze.Tests.Core.ControlStructures;

public class ControlStructuresTests
{
    [Fact]
    public void Evaluate_ContinueStatementOutsideLoop_ThrowsUnexpectedStatementException()
    {
        Assert.Throws<UnexpectedStatementException>(() =>
        {
            RazeScript.Evaluate("continue", "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_BreakStatementOutsideLoop_ThrowsUnexpectedStatementException()
    {
        Assert.Throws<UnexpectedStatementException>(() =>
        {
            RazeScript.Evaluate("break", "Raze.Tests");
        });

        Assert.Throws<UnexpectedStatementException>(() =>
        {
            RazeScript.Evaluate("""
            def integer inner() {
                break;
            }

            while (true) {
                inner();
            }
            """, "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_ReturnStatementOutsideFunction_ThrowsUnexpectedStatementException()
    {
        Assert.Throws<UnexpectedStatementException>(() =>
        {
            RazeScript.Evaluate("return", "Raze.Tests");
        });
    }
}
