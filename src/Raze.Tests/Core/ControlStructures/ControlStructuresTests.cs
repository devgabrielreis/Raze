using Raze.Script.Core.Exceptions.RuntimeExceptions;

namespace Raze.Tests.Core.ControlStructures;

public class ControlStructuresTests
{
    [Fact]
    public void Evaluate_ContinueStatementOutsideLoop_ThrowsUnexpectedStatementException()
    {
        RazeAssert.ReturnsError<UnexpectedStatementException>("continue");
    }

    [Fact]
    public void Evaluate_BreakStatementOutsideLoop_ThrowsUnexpectedStatementException()
    {
        RazeAssert.ReturnsError<UnexpectedStatementException>("break");

        var script = """
            def integer inner() {
                break;
            }

            while (true) {
                inner();
            }
        """;
        RazeAssert.ReturnsError<UnexpectedStatementException>(script);
    }

    [Fact]
    public void Evaluate_ReturnStatementOutsideFunction_ThrowsUnexpectedStatementException()
    {
        RazeAssert.ReturnsError<UnexpectedStatementException>("return");
    }
}
