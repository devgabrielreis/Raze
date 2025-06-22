using System.Reflection;
using Raze.Script.Core;
using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Values;

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
        Console.WriteLine($"Raze.Core    version {RazeScript.Version.ToString()}");
        Console.WriteLine($"Raze.Console version {Version.ToString()}");

#if !RELEASE
        Console.WriteLine($"[development build]");
#endif

        Console.WriteLine();

        InterpreterScope scope = new();

        while (true)
        {
            string command = GetCommandFromUser();

            if (command == "exit()")
            {
                break;
            }

            try
            {
                var result = RazeScript.Evaluate(command, scope);

                if (result is not VoidValue)
                {
                    Console.WriteLine(result);
                }
            }
            catch (RazeException razeEx)
            {
                PrettyPrintRazeException(razeEx, command);
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
            for (int i = 0; i < level; i++)
            {
                Console.Write("    ");
                command += "    ";
            }

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

    private static void PrettyPrintRazeException(RazeException ex, string source)
    {
        Console.WriteLine(ex.Message);
        
        if (ex.Line is not null && ex.Column is not null)
        {
            Console.WriteLine(source.Split('\n')[ex.Line.Value]);

            for (int i = 0; i < ex.Column; i++)
            {
                Console.Write(" ");
            }

            Console.WriteLine("^");
        }
        else
        {
            Console.WriteLine(source);
        }

        Console.WriteLine();
    }
}