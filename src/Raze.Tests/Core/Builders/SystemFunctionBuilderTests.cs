using Raze.Script.Core.Builders;
using Raze.Script.Core.Builders.Types;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Tests.Core.Builders;

public class SystemFunctionBuilderTests
{
    [Fact]
    public void Evaluate_FunctionWithoutName_ThrowsInvalidOperationException()
    {
        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasFunction(functionBuilder =>
                    {
                        functionBuilder
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
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_FunctionWithEmptyName_ThrowsInvalidOperationException()
    {
        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasFunction(functionBuilder =>
                    {
                        functionBuilder
                            .HasName("")
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
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_DeclaringFunctionNameTwice_ThrowsInvalidOperationException()
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
                            .HasName("test2")
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
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_DeclaringFunctionReturnTypeTwice_ThrowsInvalidOperationException()
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
                            .HasReturnType(TypeBuilder.Void)
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
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_DeclaringTwoParametersWithSameName_ThrowsInvalidOperationException()
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
                                    .HasName("num1")
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
    public void Evaluate_DeclaringParameterWithoutDefaultValueAfterParameterWithDefaultValue_ThrowsInvalidOperationException()
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
                                    .HasDefaultValue((Int128)10);
                            })
                            .HasParameter(parameterBuilder =>
                            {
                                parameterBuilder
                                    .HasName("num2")
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
    public void Evaluate_DeclaringFunctionBodyTwice_ThrowsInvalidOperationException()
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
    public void Evaluate_DeclaringFunctionWithoutReturnType_ThrowsInvalidOperationException()
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
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_DeclaringFunctionWithoutBody_ThrowsInvalidOperationException()
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
    public void Evaluate_FunctionReturningWrongType_ThrowsUnexpectedReturnType()
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
                                return RazeFunctionReturnValue.FromValue("string");
                            });
                    });
            }
        };

        string script = $"""
            import testModule;

            testModule::testFunction(10, 10)
        """;

        RazeAssert.ReturnsError<UnexpectedReturnType>(script, customModules: customModules);
    }
}
