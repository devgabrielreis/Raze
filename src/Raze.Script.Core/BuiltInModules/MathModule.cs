using Raze.Script.Core.Builders;
using Raze.Script.Core.Builders.Types;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.BuiltInModules;

internal static class MathModule
{
    internal const string Name = "math";

    internal static void Build(ModuleBuilder builder)
    {
        builder
            .HasConstant(constantBuilder =>
            {
                constantBuilder
                    .HasName("PI")
                    .HasType(TypeBuilder.Decimal)
                    .HasValue(3.14159m);
            })
            .HasConstant(constantBuilder =>
            {
                constantBuilder
                    .HasName("E")
                    .HasType(TypeBuilder.Decimal)
                    .HasValue(2.71828m);
            })
            .HasConstant(constantBuilder =>
            {
                constantBuilder
                    .HasName("MAX_INTEGER")
                    .HasType(TypeBuilder.Integer)
                    .HasValue(Int128.MaxValue);
            })
            .HasConstant(constantBuilder =>
            {
                constantBuilder
                    .HasName("MIN_INTEGER")
                    .HasType(TypeBuilder.Integer)
                    .HasValue(Int128.MinValue);
            })
            .HasConstant(constantBuilder =>
            {
                constantBuilder
                    .HasName("MAX_DECIMAL")
                    .HasType(TypeBuilder.Decimal)
                    .HasValue(decimal.MaxValue);
            })
            .HasConstant(constantBuilder =>
            {
                constantBuilder
                    .HasName("MIN_DECIMAL")
                    .HasType(TypeBuilder.Decimal)
                    .HasValue(decimal.MinValue);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("integerToDecimal")
                    .HasReturnType(TypeBuilder.Decimal)
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("num")
                            .HasType(TypeBuilder.Integer);
                    })
                    .HasBody(IntegerToDecimal);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("ceil")
                    .HasReturnType(TypeBuilder.Integer)
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("num")
                            .HasType(TypeBuilder.Decimal);
                    })
                    .HasBody(Ceil);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("floor")
                    .HasReturnType(TypeBuilder.Integer)
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("num")
                            .HasType(TypeBuilder.Decimal);
                    })
                    .HasBody(Floor);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("round")
                    .HasReturnType(TypeBuilder.Integer)
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("num")
                            .HasType(TypeBuilder.Decimal);
                    })
                    .HasBody(Round);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("roundToDecimalPlaces")
                    .HasReturnType(TypeBuilder.Decimal)
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("num")
                            .HasType(TypeBuilder.Decimal);
                    })
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("decimalPlaces")
                            .HasType(TypeBuilder.Integer);
                    })
                    .HasBody(RoundToDecimalPlaces);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("maxInteger")
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
                            .HasType(TypeBuilder.Integer);
                    })
                    .HasBody(MaxInteger);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("maxDecimal")
                    .HasReturnType(TypeBuilder.Decimal)
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("num1")
                            .HasType(TypeBuilder.Decimal);
                    })
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("num2")
                            .HasType(TypeBuilder.Decimal);
                    })
                    .HasBody(MaxDecimal);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("minInteger")
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
                            .HasType(TypeBuilder.Integer);
                    })
                    .HasBody(MinInteger);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("minDecimal")
                    .HasReturnType(TypeBuilder.Decimal)
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("num1")
                            .HasType(TypeBuilder.Decimal);
                    })
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("num2")
                            .HasType(TypeBuilder.Decimal);
                    })
                    .HasBody(MinDecimal);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("absInteger")
                    .HasReturnType(TypeBuilder.Integer)
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("num")
                            .HasType(TypeBuilder.Integer);
                    })
                    .HasBody(AbsInteger);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("absDecimal")
                    .HasReturnType(TypeBuilder.Decimal)
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("num")
                            .HasType(TypeBuilder.Decimal);
                    })
                    .HasBody(AbsDecimal);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("clampInteger")
                    .HasReturnType(TypeBuilder.Integer)
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("value")
                            .HasType(TypeBuilder.Integer);
                    })
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("min")
                            .HasType(TypeBuilder.Integer);
                    })
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("max")
                            .HasType(TypeBuilder.Integer);
                    })
                    .HasBody(ClampInteger);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("clampDecimal")
                    .HasReturnType(TypeBuilder.Decimal)
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("value")
                            .HasType(TypeBuilder.Decimal);
                    })
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("min")
                            .HasType(TypeBuilder.Decimal);
                    })
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("max")
                            .HasType(TypeBuilder.Decimal);
                    })
                    .HasBody(ClampDecimal);
            });
    }

    private static RazeFunctionReturnValue IntegerToDecimal(RazeFunctionParameters parameters)
    {
        var num = (Int128)parameters.Get("num")!;

        decimal result = (decimal)num;

        return RazeFunctionReturnValue.FromValue(result);
    }

    private static RazeFunctionReturnValue Ceil(RazeFunctionParameters parameters)
    {
        var num = (decimal)parameters.Get("num")!;

        Int128 result = (Int128)Math.Ceiling(num);

        return RazeFunctionReturnValue.FromValue(result);
    }

    private static RazeFunctionReturnValue Floor(RazeFunctionParameters parameters)
    {
        var num = (decimal)parameters.Get("num")!;

        Int128 result = (Int128)Math.Floor(num);

        return RazeFunctionReturnValue.FromValue(result);
    }

    private static RazeFunctionReturnValue Round(RazeFunctionParameters parameters)
    {
        var num = (decimal)parameters.Get("num")!;

        Int128 result = (Int128)Math.Round(num);

        return RazeFunctionReturnValue.FromValue(result);
    }

    private static RazeFunctionReturnValue RoundToDecimalPlaces(RazeFunctionParameters parameters)
    {
        var num = (decimal)parameters.Get("num")!;
        var decimalPlaces = (Int128)parameters.Get("decimalPlaces")!;

        var decimalPlacesInt = (int)decimalPlaces;

        decimalPlacesInt = Math.Clamp(decimalPlacesInt, 0, 28);

        decimal result = Math.Round(num, decimalPlacesInt);

        return RazeFunctionReturnValue.FromValue(result);
    }

    private static RazeFunctionReturnValue MaxInteger(RazeFunctionParameters parameters)
    {
        var num1 = (Int128)parameters.Get("num1")!;
        var num2 = (Int128)parameters.Get("num2")!;

        Int128 result = (num1 > num2) ? num1 : num2;

        return RazeFunctionReturnValue.FromValue(result);
    }

    private static RazeFunctionReturnValue MaxDecimal(RazeFunctionParameters parameters)
    {
        var num1 = (decimal)parameters.Get("num1")!;
        var num2 = (decimal)parameters.Get("num2")!;

        decimal result = (num1 > num2) ? num1 : num2;

        return RazeFunctionReturnValue.FromValue(result);
    }

    private static RazeFunctionReturnValue MinInteger(RazeFunctionParameters parameters)
    {
        var num1 = (Int128)parameters.Get("num1")!;
        var num2 = (Int128)parameters.Get("num2")!;

        Int128 result = (num1 < num2) ? num1 : num2;

        return RazeFunctionReturnValue.FromValue(result);
    }

    private static RazeFunctionReturnValue MinDecimal(RazeFunctionParameters parameters)
    {
        var num1 = (decimal)parameters.Get("num1")!;
        var num2 = (decimal)parameters.Get("num2")!;

        decimal result = (num1 < num2) ? num1 : num2;

        return RazeFunctionReturnValue.FromValue(result);
    }

    private static RazeFunctionReturnValue AbsInteger(RazeFunctionParameters parameters)
    {
        var num = (Int128)parameters.Get("num")!;

        var result = (num < 0) ? (num * -1) : num;

        return RazeFunctionReturnValue.FromValue(result);
    }

    private static RazeFunctionReturnValue AbsDecimal(RazeFunctionParameters parameters)
    {
        var num = (decimal)parameters.Get("num")!;

        decimal result = (decimal)Math.Abs(num);

        return RazeFunctionReturnValue.FromValue(result);
    }

    private static RazeFunctionReturnValue ClampInteger(RazeFunctionParameters parameters)
    {
        var value = (Int128)parameters.Get("value")!;
        var min = (Int128)parameters.Get("min")!;
        var max = (Int128)parameters.Get("max")!;

        Int128 result;

        if (value < min)
        {
            result = min;
        }
        else if (value > max)
        {
            result = max;
        }
        else
        {
            result = value;
        }

        return RazeFunctionReturnValue.FromValue(result);
    }

    private static RazeFunctionReturnValue ClampDecimal(RazeFunctionParameters parameters)
    {
        var value = (decimal)parameters.Get("value")!;
        var min = (decimal)parameters.Get("min")!;
        var max = (decimal)parameters.Get("max")!;

        decimal result =  Math.Clamp(value, min, max);

        return RazeFunctionReturnValue.FromValue(result);
    }
}
