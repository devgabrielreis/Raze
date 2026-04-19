using Raze.Script.Core.Builders;
using Raze.Script.Core.Builders.Types;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Tests.Core.Builders;

public class ParameterBuilderTests
{
    [Fact]
    public void Evaluate_DeclaringParameterWithoutName_ThrowsInvalidOperationException()
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
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_ParameterWithEmptyName_ThrowsInvalidOperationException()
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
                                    .HasName("")
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
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_DeclaringParameterTypeTwice_ThrowsInvalidOperationException()
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
                                    .HasType(TypeBuilder.Integer)
                                    .HasType(TypeBuilder.String);
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
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_DeclaringParameterDefaultValueBeforeType_ThrowsInvalidOperationException()
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
                                    .HasDefaultValue((Int128)10)
                                    .HasType(TypeBuilder.Integer);
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
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_DeclaringParameterDefaultValueTwice_ThrowsInvalidOperationException()
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
                                    .HasDefaultValue((Int128)10)
                                    .HasNullDefaultValue();
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
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_DeclaringIncompatibleParameterDefaultValue_ThrowsInvalidOperationException()
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
                                    .HasDefaultValue("string");
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
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_DeclaringParameterWithoutType_ThrowsInvalidOperationException()
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
                                    .HasName("num1");
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
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }
}
