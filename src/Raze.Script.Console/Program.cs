using Raze.Script.Console;

internal class Program
{
    internal static Version Version => typeof(Program).Assembly.GetName().Version!;

    private static void Main(string[] args)
    {
        RazeConsole.RunInterpreter();
    }
}