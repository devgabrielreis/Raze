namespace Raze.Tests.Core.Variables;

public class FunctionVariableTests
{
    [Fact]
    public void Evaluate_Function_CanBeStoredInVariables()
    {
        var script = """
            def string myFunc(integer param, integer? param2 = 15) {
                return "hello";
            }

            var function<integer, integer?, string> myFuncVar = myFunc;
            myFuncVar(1);
        """;

        RazeAssert.EvaluatesToString(script, "hello");
    }
}
