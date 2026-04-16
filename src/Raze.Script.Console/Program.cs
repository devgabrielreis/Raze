using Raze.Script.Console;
using System.CommandLine;
using System.Globalization;

internal class Program
{
    internal static Version Version => typeof(Program).Assembly.GetName().Version!;

    internal static int Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

        var rootCommand = new RootCommand("Raze Script Console - A script runner and REPL for the Raze language");

        var versionOption = new Option<bool>("--version", "-v")
        {
            Description = "Show version information"
        };

        RemoveDefaultVersionOption(rootCommand);
        rootCommand.Options.Add(versionOption);

        rootCommand.SetAction(parseResult =>
        {
            if (parseResult.GetValue(versionOption))
            {
                Console.WriteLine(Version.ToString());
            }
            else
            {
                RazeConsole.RunInterpreter();
            }
        });

        return rootCommand.Parse(args).Invoke();
    }

    private static void RemoveDefaultVersionOption(RootCommand rootCommand)
    {
        var helpOption = rootCommand.Options.FirstOrDefault(
            o => o.Name == "--version"
        );

        if (helpOption != null)
        {
            rootCommand.Options.Remove(helpOption);
        }
    }
}