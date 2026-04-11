using Raze.Script.Core.Builders;
using Raze.Script.Core.Builders.Types;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Console;

internal static class ConsoleModule
{
    internal const string Name = "console";

    internal static void Build(ModuleBuilder builder)
    {
        builder
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("writeLine")
                    .HasReturnType(TypeBuilder.Void)
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("message")
                            .HasType(TypeBuilder.String);
                    })
                    .HasBody(WriteLine);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("write")
                    .HasReturnType(TypeBuilder.Void)
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("message")
                            .HasType(TypeBuilder.String);
                    })
                    .HasBody(Write);
            })
            .HasFunction(functionBuilder =>
            {
                functionBuilder
                    .HasName("readLine")
                    .HasReturnType(TypeBuilder.String.AsNullable())
                    .HasParameter(parameterBuilder =>
                    {
                        parameterBuilder
                            .HasName("prompt")
                            .HasType(TypeBuilder.String.AsNullable())
                            .HasNullDefaultValue();
                    })
                    .HasBody(ReadLine);
            });
    }

    private static RazeFunctionReturnValue WriteLine(RazeFunctionParameters parameters)
    {
        var message = (string)parameters.Get("message")!;

        System.Console.WriteLine(message);

        return RazeFunctionReturnValue.Void;
    }

    private static RazeFunctionReturnValue Write(RazeFunctionParameters parameters)
    {
        var message = (string)parameters.Get("message")!;

        System.Console.Write(message);

        return RazeFunctionReturnValue.Void;
    }

    private static RazeFunctionReturnValue ReadLine(RazeFunctionParameters parameters)
    {
        var prompt = (string?)parameters.Get("prompt");

        if (prompt != null)
        {
            System.Console.Write(prompt);
        }

        var result = System.Console.ReadLine();

        return (result == null)
                ? RazeFunctionReturnValue.Null
                : RazeFunctionReturnValue.FromValue((string)result);
    }
}
