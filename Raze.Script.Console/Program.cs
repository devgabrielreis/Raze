using Raze.Script.Core;
using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;
using System.Reflection;

internal class Program
{
    public static Version Version => Assembly.GetExecutingAssembly()
                                             .GetName()
                                             .Version!;

    private static void Main(string[] args)
    {
        RunInterpreter();
    }

    private static void RunInterpreter()
    {
        Console.WriteLine("Raze Interpreter");
        Console.WriteLine($"Raze.Console version {Version.ToString()}");

#if !RELEASE
        Console.WriteLine($"[development build]");
#endif

        Console.WriteLine();

        InterpreterScope scope = new();
        var sources = new Dictionary<string, string>();

        while (true)
        {
            string command = GetCommandFromUser();

            if (command == "exit()")
            {
                break;
            }

            var sourceName = $"interpreter-source-{sources.Count}";
            sources[sourceName] = command;

            try
            {
                var result = RazeScript.Evaluate(command, sourceName, scope);

                if (result is not VoidValue)
                {
                    Console.WriteLine(result);
                }
            }
            catch (RazeException razeEx)
            {
                PrettyPrintRazeException(razeEx, sources);
            }
        }
    }

    private static string GetCommandFromUser()
    {
        Console.Write("> ");
        string command = Console.ReadLine()!;

        int level;
        while ((level = GetIndentationLevel(command)) > 0)
        {
            command += '\n';

            Console.Write("..");
            command += Console.ReadLine()!;
        }

        return command;
    }

    public static int GetIndentationLevel(string command)
    {
        int level = 0;

        foreach (var ch in command)
        {
            if (ch == '{')
            {
                level++;
            }
            else if (ch == '}')
            {
                level--;
            }
        }

        return level;
    }

    private static void PrettyPrintRazeException(RazeException ex, IReadOnlyDictionary<string, string> sources)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
        Console.WriteLine();

        if (!sources.TryGetValue(ex.SourceInfo.Location, out string? source))
        {
            return;
        }

        Console.WriteLine($"At \"{ex.SourceInfo.Location}\".");
        
        if (ex.SourceInfo.SourcePosition is not null)
        {
            Console.WriteLine($"Line {ex.SourceInfo.SourcePosition.Line}, column {ex.SourceInfo.SourcePosition.Column}.");
            Console.WriteLine();

            string errorLine = source.Split('\n')[ex.SourceInfo.SourcePosition.Line - 1];
            Console.WriteLine(errorLine);

            for (int i = 0; i < ex.SourceInfo.SourcePosition.Column - 1; i++)
            {
                Console.Write(errorLine[i] == '\t' ? '\t' : ' ');
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine('^');
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine(source);
        }

        Console.WriteLine();
    }
}