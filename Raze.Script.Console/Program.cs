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
            Console.Write("> ");
            string command = Console.ReadLine()!;

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
            catch (Exception ex)
            {
                if (ex is RazeException razeEx)
                {
                    PrettyPrintRazeException(razeEx, command);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    private static void PrettyPrintRazeException(RazeException ex, string source)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(source);
        
        if (ex.Line is not null && ex.Column is not null)
        {
            for (int i = 0; i < ex.Column; i++)
            {
                Console.Write(" ");
            }

            Console.WriteLine("^");
        }

        Console.WriteLine();
    }
}