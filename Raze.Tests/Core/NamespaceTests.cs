using Raze.Script.Core;
using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Tests.Core;

public class NamespaceTests
{
    [Fact]
    public void Evaluate_Namespaces_CanHaveConstantDeclaration()
    {
        var script = """
            namespace test {
                const string MY_STRING = "test";
            }

            test::MY_STRING
        """;

        var result = RazeScript.Evaluate(script, "Raze.Tests");

        Assert.IsType<StringValue>(result);
        Assert.Equal("test", (result as StringValue)!.StrValue);
    }

    [Fact]
    public void Evaluate_Namespaces_CanHaveFunctionDeclaration()
    {
        var script = """
            namespace test {
                def integer my_func(integer num) {
                    return num + 5;
                }
            }

            test::my_func(5)
        """;

        var result = RazeScript.Evaluate(script, "Raze.Tests");

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(10, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_DeclaringVariableInNamespace_ThrowsScopeDeclarationException()
    {
        var script = """
            namespace test {
                var string my_string = "test";
            }

            test::my_string
        """;

        Assert.Throws<ScopeDeclarationException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_DeclaringNamespaceInNamespace_ThrowsScopeDeclarationException()
    {
        var script = """
            namespace test {
                namespace inner {}
            }
        """;

        Assert.Throws<ScopeDeclarationException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_DeclaringNamespaceInLocalScope_ThrowsScopeDeclarationException()
    {
        var script = """
            {
                namespace test {
                    const decimal PI = 3.14;
                }
            }
        """;

        Assert.Throws<ScopeDeclarationException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_Script_CanHaveMultipleNamespaceDeclarations()
    {
        var script = """
            namespace first {
                const integer MY_NUM = 3;
            }

            namespace second {
                const integer MY_NUM = 5;
            }

            first::MY_NUM + second::MY_NUM
        """;

        var result = RazeScript.Evaluate(script, "Raze.Tests");

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(8, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_Namespace_CanAcessAnotherNamespace()
    {
        var script = """
            namespace first {
                const integer MY_NUM = 3;
            }

            namespace second {
                def integer sum(integer num) {
                    return num + first::MY_NUM;
                }
            }

            second::sum(10)
        """;

        var result = RazeScript.Evaluate(script, "Raze.Tests");

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(13, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_Namespace_CanBeDeclaredInMultipleBlocks()
    {
        var script = """
            namespace main {
                const integer MY_NUM1 = 3;
            }

            namespace main {
                const integer MY_NUM2 = 0;
            }

            main::MY_NUM1 + main::MY_NUM2
        """;

        var result = RazeScript.Evaluate(script, "Raze.Tests");

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(3, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_RedeclaringIdentifierInNamespace_ThrowsRedeclarationException()
    {
        var script = """
            namespace main {
                const integer test = 3;
            }

            namespace main {
                def void test() {}
            }
        """;

        Assert.Throws<RedeclarationException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_Namespace_ShouldBeAbleToAccessMembersInReopenedNamespace()
    {
        var script = """
            namespace main {
                const integer test = 7;

                def integer get_test() {
                    return test;
                }
            }

            namespace main {
                def integer super_get_test() {
                    return get_test();
                }
            }

            main::super_get_test()
        """;

        var result = RazeScript.Evaluate(script, "Raze.Tests");

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(7, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_Namespace_ShouldSupportForwardReferences()
    {
        var script = """
            namespace main {
                def integer call_later() {
                    return get_value();
                }

                def integer get_value() {
                    return 42;
                }
            }

            main::call_later()
        """;

        var result = RazeScript.Evaluate(script, "Raze.Tests");
        Assert.Equal(42, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_Namespace_LocalVariableShouldShadowNamespaceConstant()
    {
        var script = """
            namespace main {
                const integer value = 10;

                def integer get_value() {
                    const integer value = 20; 
                    return value;
                }
            }

            main::get_value()
        """;

        var result = RazeScript.Evaluate(script, "Raze.Tests");
        Assert.Equal(20, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_Namespace_ShouldPrioritizeInternalMemberOverGlobal()
    {
        var script = """
            const integer test = 1;

            namespace main {
                const integer test = 99;

                def integer get_test() {
                    return test;
                }
            }

            main::get_test()
        """;

        var result = RazeScript.Evaluate(script, "Raze.Tests");
        Assert.Equal(99, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_AccessingMissingMemberInNamespace_ThrowsUndefinedException()
    {
        var script = """
            namespace test {}

            test::what
        """;

        Assert.Throws<UndefinedIdentifierException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
    }

    [Fact]
    public void Evaluate_Namespaces_ShouldSupportMutualDependency()
    {
        var script = """
            namespace first {
                const integer MY_NUM = 3;

                def integer better_sum(integer num1, integer num2) {
                    return num1 + num2 + second::sum(4);
                }
            }

            namespace second {
                def integer sum(integer num) {
                    return num + first::MY_NUM;
                }
            }

            first::better_sum(10, 5)
        """;

        var result = RazeScript.Evaluate(script, "Raze.Tests");

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(22, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_NamespaceAccessWithInvalidMember_ThrowsUnexpectedTokenException()
    {
        var script = """
            namespace main {}

            main::10
        """;

        Assert.Throws<UnexpectedTokenException>(() =>
        {
            RazeScript.Evaluate(script, "Raze.Tests");
        });
    }
}
