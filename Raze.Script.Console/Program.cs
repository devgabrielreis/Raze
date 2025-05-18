using Raze.Script.Core;
using Raze.Script.Core.Exceptions;

internal class Program
{
    private static void Main(string[] args)
    {
        RunInterpreter();
    }

    private static void RunInterpreter()
    {
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
                Console.WriteLine(RazeScript.Evaluate(command));
            }
            catch (Exception ex)
            {
                if (ex is RazeException)
                {
                    Console.WriteLine(ex.Message);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}