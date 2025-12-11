using Raze.Script.Core;
using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;

namespace Raze.Tests.Core;

public class FunctionTests
{
    [Theory]
    [InlineData(@"
        def integer myFunc() {
            return 10;
        }
        
        myFunc();
    ", "10")]
    [InlineData(@"
        def decimal myFunc(decimal param) {
            return param + 5.0;
        }
        
        myFunc(5.0);
    ", "10.0")]
    [InlineData(@"
        def integer? myFunc(integer param, integer param2) {
            var integer result = param + param2;
            
            if (result == 15) {
                return null;
            }

            return result;
        }
        
        myFunc(5, 10);
    ", "null")]
    [InlineData(@"
        def integer? myFunc(integer param, integer param2) {
            var integer result = param + param2;
            
            if (result == 15) {
                return null;
            }

            return result;
        }
        
        myFunc(10, 10);
    ", "20")]
    public void Evaluate_Function_ReturnsExpectedValue(string script, string expectedResult)
    {
        var result = RazeScript.Evaluate(script, "Raze.Tests");

        Assert.Equal(expectedResult, result.ToString());
    }

    [Fact]
    public void Evaluate_Parameter_CanHaveDefaultValue()
    {
        var function = @"
            def integer myFunc(integer param, const integer param2 = 15) {
                return param + param2;
            }
        ";
        var scope = new InterpreterScope();
        RazeScript.Evaluate(function, "Raze.Tests", scope);

        var result = RazeScript.Evaluate("myFunc(5)", "Raze.Tests", scope);
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(20, (result as IntegerValue)!.IntValue);

        result = RazeScript.Evaluate("myFunc(5, 25)", "Raze.Tests", scope);
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(30, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_Function_CanHaveVoidReturn()
    {
        var script = @"
            def void myFunc() {
                return;
            }

            myFunc();
        ";

        var result = RazeScript.Evaluate(script, "Raze.Tests");
        Assert.IsType<VoidValue>(result);

        script = @"
            def void myFunc() {
            }

            myFunc();
        ";

        result = RazeScript.Evaluate(script, "Raze.Tests");
        Assert.IsType<VoidValue>(result);
    }

    [Fact]
    public void Evaluate_Closures_KeepTheirParentScope()
    {
        var script = @"
            def function<integer> makeCounter(integer start) {
                def integer closure() {
                    start = start + 1;
                    return start;
                }
                return closure;
            }

            const function<integer> counter1 = makeCounter(100);
            const function<integer> counter2 = makeCounter(50);
        ";

        var scope = new InterpreterScope();
        RazeScript.Evaluate(script, "Raze.Tests", scope);

        var result = RazeScript.Evaluate("counter2()", "Raze.Tests", scope);
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(51, (result as IntegerValue)!.IntValue);

        result = RazeScript.Evaluate("counter1()", "Raze.Tests", scope);
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(101, (result as IntegerValue)!.IntValue);

        result = RazeScript.Evaluate("counter2()", "Raze.Tests", scope);
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(52, (result as IntegerValue)!.IntValue);

        result = RazeScript.Evaluate("counter1()", "Raze.Tests", scope);
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(102, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_Function_CanReceiveAnotherFunctionAsParameter()
    {
        var script = @"
            def integer myFunc(function<integer> funcParam) {
                return 30 + funcParam();
            }

            def integer myFuncParam() {
                return 20;
            }

            myFunc(myFuncParam);
        ";

        var result = RazeScript.Evaluate(script, "Raze.Tests");
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(50, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_IncompleteParameterList_ThrowsInvalidParameterListException()
    {
        var script = @"
            def integer myFunc(integer param, integer param2 = 15) {
                return param + param2;
            }

            myFunc();
        ";

        Assert.Throws<InvalidParameterListException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_AssignToConstantParameter_ThrowsConstantAssignmentException()
    {
        var script = @"
            def integer myFunc(integer param, const integer param2) {
                param = 50;
                param2 = 100;

                return param + param2;
            }

            myFunc(10, 10);
        ";

        Assert.Throws<ConstantAssignmentException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_AssignToDeclaredFunction_ThrowsConstantAssignmentException()
    {
        var script = @"
            def integer myFunc(const integer param, const integer param2) {
                return param + param2;
            }

            def integer myFunc2(const integer param, const integer param2) {
                return 10;
            }

            myFunc = myFunc2;
        ";

        Assert.Throws<ConstantAssignmentException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_DeclaringNonOptionalParameterAfterOptionalParameter_ThrowsInvalidParameterListException()
    {
        var script = @"
            def integer myFunc(integer param = 10, integer param2) {
                return 10;
            }
        ";

        Assert.Throws<InvalidParameterListException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_FunctionReturningWrongType_ThrowsUnexpectedTypeException()
    {
        var script = @"
            def integer myFunc() {
                return 10.0;
            }

            myFunc();
        ";

        Assert.Throws<UnexpectedTypeException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_FunctionCalls_CanBeChained()
    {
        var script = @"
            def function<integer> makeCounter(integer start) {
                def integer closure() {
                    start = start + 1;
                    return start;
                }
                return closure;
            }

            makeCounter(0)();
        ";

        var result = RazeScript.Evaluate(script, "Raze.Tests");
        Assert.IsType<IntegerValue>(result);
        Assert.Equal(1, (result as IntegerValue)!.IntValue);
    }
}
