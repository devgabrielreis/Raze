using Raze.Script.Console.CustomModules;
using Raze.Script.Console.Utils;
using Raze.Script.Core;
using Raze.Script.Core.Builders;
using Raze.Script.Core.Result;
using Raze.Script.Core.Runtime.Types;

namespace Raze.Script.Console;

internal class RazeConsole
{
    private static readonly Dictionary<string, Action<ModuleBuilder>> customModules = new()
    {
        [ConsoleModule.Name] = ConsoleModule.Build
    };

    internal static int ExecuteScript(FileInfo script)
    {
        var result = RazeScript.ExecuteScript(script, customModules);

        if (result is RazeError razeError)
        {
            RazeUtils.PrettyPrintRazeException(razeError.Error);
            return 1;
        }

        if (result is RazeSuccess razeSuccess && razeSuccess.ValueType == BaseType.Integer)
        {
            return (int)razeSuccess.AsInteger();
        }

        return 0;
    }

    internal static void RunInterpreter()
    {
        PrintHeader();

        var scope = RazeScript.CreateInterpreterScope();
        var sources = new Dictionary<string, string>();

        while (true)
        {
            string command = GetCommandFromUser();

            var sourceName = $"interpreter-source-{sources.Count}";
            sources[sourceName] = command;

            var result = RazeScript.Evaluate(command, sourceName, scope, customModules);

            if (result is RazeSuccess success && success.ValueType != BaseType.Void)
            {
                System.Console.WriteLine(success.ValueString);
            }
            else if (result is RazeError error)
            {
                RazeUtils.PrettyPrintRazeException(error.Error, sources);
            }
        }
    }

    private static void PrintHeader()
    {
        System.Console.WriteLine("Raze Interpreter");
        System.Console.WriteLine($"Raze.Console version {Program.Version}");

#if !RELEASE
        System.Console.WriteLine($"[development build]");
#endif

        System.Console.WriteLine();
    }

    private static string GetCommandFromUser()
    {
        System.Console.Write("> ");
        string command = System.Console.ReadLine()!;

        while (RazeUtils.GetScriptIndentationLevel(command) > 0)
        {
            command += '\n';

            System.Console.Write("..");
            command += System.Console.ReadLine()!;
        }

        return command;
    }
}
