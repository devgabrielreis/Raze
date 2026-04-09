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
}
