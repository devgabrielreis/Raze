using Raze.Script.Core.Builders;
using Raze.Script.Core.Builders.Types;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Tests.Core.Builders;

public class ModuleBuilderTests
{
    [Fact]
    public void Evaluate_ModuleBuilderConstant_ReturnsExpectedValue()
    {
        Int128 expected = 10;

        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasConstant(constantBuilder =>
                    {
                        constantBuilder
                            .HasName("testConstant")
                            .HasType(TypeBuilder.Integer)
                            .HasValue(expected);
                    });
            }
        };

        string script = $"""
            import testModule;

            testModule::testConstant
        """;

        RazeAssert.EvaluatesToInteger(script, expected, customModules: customModules);
    }

    [Fact]
    public void Evaluate_ModuleBuilderFunction_ReturnsExpectedValue()
    {
        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasFunction(functionBuilder =>
                    {
                        functionBuilder
                            .HasName("testFunction")
                            .HasReturnType(TypeBuilder.Integer)
                            .HasParameter(parameterBuilder =>
                            {
                                parameterBuilder
                                    .HasName("num1")
                                    .HasType(TypeBuilder.Integer);
                            })
                            .HasParameter(parameterBuilder =>
                            {
                                parameterBuilder
                                    .HasName("num2")
                                    .HasType(TypeBuilder.Integer)
                                    .HasDefaultValue((Int128)10);
                            })
                            .HasBody(razeParams =>
                            {
                                var num1 = (Int128)razeParams.Get("num1")!;
                                var num2 = (Int128)razeParams.Get("num2")!;

                                Int128 result = num1 + num2;

                                return RazeFunctionReturnValue.FromValue(result);
                            });
                    });
            }
        };

        string script = $"""
            import testModule;

            testModule::testFunction(10)
        """;

        RazeAssert.EvaluatesToInteger(script, 20, customModules: customModules);
    }

    [Fact]
    public void Evaluate_DefiningModuleWithRepeatedMemberNames_ThrowsInvalidOperationException()
    {
        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasConstant(constantBuilder =>
                    {
                        constantBuilder
                            .HasName("sameConst")
                            .HasType(TypeBuilder.Integer)
                            .HasValue((Int128)10);
                    })
                    .HasConstant(constantBuilder =>
                    {
                        constantBuilder
                            .HasName("sameConst")
                            .HasType(TypeBuilder.String)
                            .HasValue("10");
                    });
            }
        };

        string script = $"""
            import testModule;
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }
}
