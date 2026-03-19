using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Runtime.Scopes;
using Raze.Script.Core.Runtime.Types;
using System.Globalization;

namespace Raze.Tests.Core;

public class FunctionTests
{
    [Theory]
    [InlineData(@"
        def integer myFunc() {
            return 10;
        }
        
        myFunc();
    ", 10, BaseType.Integer)]
    [InlineData(@"
        def decimal myFunc(decimal param) {
            return param + 5.0;
        }
        
        myFunc(5.0);
    ", "10.0", BaseType.Decimal)]
    [InlineData(@"
        def integer? myFunc(integer param, integer param2) {
            var integer result = param + param2;
            
            if (result == 15) {
                return null;
            }

            return result;
        }
        
        myFunc(5, 10);
    ", null, BaseType.Null)]
    [InlineData(@"
        def integer? myFunc(integer param, integer param2) {
            var integer result = param + param2;
            
            if (result == 15) {
                return null;
            }

            return result;
        }
        
        myFunc(10, 10);
    ", 20, BaseType.Integer)]
    public void Evaluate_Function_ReturnsExpectedValue(string script, object? expectedResult, BaseType expectedType)
    {
        switch (expectedType)
        {
            case BaseType.Integer:
                RazeAssert.EvaluatesToInteger(script, (int)expectedResult!);
                break;
            case BaseType.Decimal:
                decimal expectedDecimal = decimal.Parse((string)expectedResult!, CultureInfo.InvariantCulture);
                RazeAssert.EvaluatesToDecimal(script, expectedDecimal);
                break;
            case BaseType.Null:
                RazeAssert.EvaluatesToNull(script);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(expectedType));
        }
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

        RazeAssert.EvaluatesToVoid(function, scope);
        RazeAssert.EvaluatesToInteger("myFunc(5)", 20, scope);
        RazeAssert.EvaluatesToInteger("myFunc(5, 25)", 30, scope);
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

        RazeAssert.EvaluatesToVoid(script);

        script = @"
            def void myFunc() {
            }

            myFunc();
        ";

        RazeAssert.EvaluatesToVoid(script);
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

        RazeAssert.EvaluatesToVoid(script, scope);
        RazeAssert.EvaluatesToInteger("counter2()", 51, scope);
        RazeAssert.EvaluatesToInteger("counter1()", 101, scope);
        RazeAssert.EvaluatesToInteger("counter2()", 52, scope);
        RazeAssert.EvaluatesToInteger("counter1()", 102, scope);
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

        RazeAssert.EvaluatesToInteger(script, 50);
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

        RazeAssert.ReturnsError<InvalidParameterListException>(script);
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

        RazeAssert.ReturnsError<ConstantAssignmentException>(script);
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

        RazeAssert.ReturnsError<ConstantAssignmentException>(script);
    }

    [Fact]
    public void Evaluate_DeclaringNonOptionalParameterAfterOptionalParameter_ThrowsInvalidParameterListException()
    {
        var script = @"
            def integer myFunc(integer param = 10, integer param2) {
                return 10;
            }
        ";

        RazeAssert.ReturnsError<InvalidParameterListException>(script);
    }

    [Fact]
    public void Evaluate_FunctionReturningWrongType_ThrowsUnexpectedReturnType()
    {
        var script = @"
            def integer myFunc() {
                return 10.0;
            }

            myFunc();
        ";

        RazeAssert.ReturnsError<UnexpectedReturnType>(script);
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

        RazeAssert.EvaluatesToInteger(script, 1);
    }

    [Fact]
    public void Evaluate_Function_CanBeRecursive()
    {
        var script = @"
            def integer fibonacci(integer n) {
                if (n <= 1) {
                    return n;
                }

                return fibonacci(n - 1) + fibonacci(n - 2);
            }

            fibonacci(6);
        ";

        RazeAssert.EvaluatesToInteger(script, 8);
    }

    [Fact]
    public void Evaluate_NonFunctionCall_ThrowsInvalidCallExpressionException()
    {
        var script = @"
            var integer myvar = 10;

            myvar();
        ";

        RazeAssert.ReturnsError<InvalidCallExpressionException>(script);
    }
}
