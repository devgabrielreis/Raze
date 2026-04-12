using Raze.Script.Core.Builders;
using Raze.Script.Core.Builders.Types;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Tests.Core;

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

    [Fact]
    public void Evaluate_DeclaringConstantWithoutName_ThrowsInvalidOperationException()
    {
        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasConstant(constantBuilder =>
                    {
                        constantBuilder
                            .HasType(TypeBuilder.Integer)
                            .HasValue((Int128)10);
                    });
            }
        };

        string script = $"""
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_EmptyConstantName_ThrowsInvalidOperationException()
    {
        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasConstant(constantBuilder =>
                    {
                        constantBuilder
                            .HasName("")
                            .HasType(TypeBuilder.Integer)
                            .HasValue((Int128)10);
                    });
            }
        };

        string script = $"""
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_DeclaringConstantValueBeforeType_ThrowsInvalidOperationException()
    {
        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasConstant(constantBuilder =>
                    {
                        constantBuilder
                            .HasName("testConstant")
                            .HasValue((Int128)10)
                            .HasType(TypeBuilder.Integer);
                    });
            }
        };

        string script = $"""
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_DeclaringConstantValueTwice_ThrowsInvalidOperationException()
    {
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
                            .HasValue((Int128)10)
                            .HasValue((Int128)20);
                    });
            }
        };

        string script = $"""
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_DeclaringConstantWithIncompatibleValue_ThrowsInvalidOperationException()
    {
        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasConstant(constantBuilder =>
                    {
                        constantBuilder
                            .HasName("testConstant")
                            .HasType(TypeBuilder.String)
                            .HasValue((Int128)10);
                    });
            }
        };

        string script = $"""
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_DeclaringConstantWithoutValue_ThrowsInvalidOperationException()
    {
        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasConstant(constantBuilder =>
                    {
                        constantBuilder
                            .HasName("testConstant")
                            .HasType(TypeBuilder.Integer);
                    });
            }
        };

        string script = $"""
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

    [Fact]
    public void Evaluate_DeclaringNullableVoidType_ThrowsInvalidOperationException()
    {
        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasConstant(constantBuilder =>
                    {
                        constantBuilder
                            .HasName("testConstant")
                            .HasType(TypeBuilder.Void.AsNullable())
                            .HasNullValue();
                    });
            }
        };

        string script = $"""
            import testModule
        """;

        RazeAssert.ThrowsError<InvalidOperationException>(script, customModules: customModules);
    }

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
