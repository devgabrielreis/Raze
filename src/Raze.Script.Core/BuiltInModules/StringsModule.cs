using Raze.Script.Core.Builders;
using Raze.Script.Core.Builders.Types;
using Raze.Script.Core.Runtime.Values;
using System.Globalization;

namespace Raze.Script.Core.BuiltInModules;

internal static class StringsModule
{
    internal const string Name = "strings";

    internal static void Build(ModuleBuilder builder)
    {
        builder
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("integerToString")
                    .HasReturnType(TypeBuilder.String)
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("value")
                            .HasType(TypeBuilder.Integer);
                    })
                    .HasBody(IntegerToString);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("decimalToString")
                    .HasReturnType(TypeBuilder.String)
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("value")
                            .HasType(TypeBuilder.Decimal);
                    })
                    .HasBody(DecimalToString);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("booleanToString")
                    .HasReturnType(TypeBuilder.String)
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("value")
                            .HasType(TypeBuilder.Boolean);
                    })
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("ifTrue")
                            .HasType(TypeBuilder.String)
                            .HasDefaultValue("1");
                    })
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("ifFalse")
                            .HasType(TypeBuilder.String)
                            .HasDefaultValue("0");
                    })
                    .HasBody(BooleanToString);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("tryStringToInteger")
                    .HasReturnType(TypeBuilder.Integer.AsNullable())
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("str")
                            .HasType(TypeBuilder.String);
                    })
                    .HasBody(TryStringToInteger);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("tryStringToDecimal")
                    .HasReturnType(TypeBuilder.Decimal.AsNullable())
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("str")
                            .HasType(TypeBuilder.String);
                    })
                    .HasBody(TryStringToDecimal);
            });
    }

    private static RazeFunctionReturnValue IntegerToString(RazeFunctionParameters parameters)
    {
        var value = (Int128)parameters.Get("value")!;

        var result = value.ToString();

        return RazeFunctionReturnValue.FromValue(result);
    }

    private static RazeFunctionReturnValue DecimalToString(RazeFunctionParameters parameters)
    {
        var value = (decimal)parameters.Get("value")!;

        var result = value.ToString(CultureInfo.InvariantCulture);
        
        result = result.TrimEnd('0');
        if (result.EndsWith('.'))
        {
            result += '0';
        }

        return RazeFunctionReturnValue.FromValue(result);
    }

    private static RazeFunctionReturnValue BooleanToString(RazeFunctionParameters parameters)
    {
        var value = (bool)parameters.Get("value")!;
        var ifTrue = (string)parameters.Get("ifTrue")!;
        var ifFalse = (string)parameters.Get("ifFalse")!;

        var result = value ? ifTrue : ifFalse;

        return RazeFunctionReturnValue.FromValue(result);
    }

    private static RazeFunctionReturnValue TryStringToInteger(RazeFunctionParameters parameters)
    {
        var str = (string)parameters.Get("str")!;

        var success = Int128.TryParse(str, out var result);

        return success
            ? RazeFunctionReturnValue.FromValue(result)
            : RazeFunctionReturnValue.Null;
    }

    private static RazeFunctionReturnValue TryStringToDecimal(RazeFunctionParameters parameters)
    {
        var str = (string)parameters.Get("str")!;

        if (str.EndsWith('.'))
        {
            return RazeFunctionReturnValue.Null;
        }

        var success = decimal.TryParse(str, CultureInfo.InvariantCulture, out var result);

        return success
            ? RazeFunctionReturnValue.FromValue(result)
            : RazeFunctionReturnValue.Null;
    }
}
