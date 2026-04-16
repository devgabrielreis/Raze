using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Console.Utils;

internal static class RazeUtils
{
    internal static int GetScriptIndentationLevel(string command)
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

    internal static void PrettyPrintRazeException(RazeException ex, IReadOnlyDictionary<string, string> sources)
    {
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine(ex.Message);
        System.Console.ResetColor();
        System.Console.WriteLine();

        if (!sources.TryGetValue(ex.SourceInfo.Location, out string? source))
        {
            return;
        }

        System.Console.WriteLine($"At \"{ex.SourceInfo.Location}\".");

        if (ex.SourceInfo.SourcePosition is SourcePosition sourcePosition)
        {
            System.Console.WriteLine($"Line {sourcePosition.Line}, column {sourcePosition.Column}.");
            System.Console.WriteLine();

            string errorLine = source.Split('\n')[sourcePosition.Line - 1];
            System.Console.WriteLine(errorLine);

            for (int i = 0; i < sourcePosition.Column - 1; i++)
            {
                System.Console.Write(errorLine[i] == '\t' ? '\t' : ' ');
            }

            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine('^');
            System.Console.ResetColor();
        }
        else
        {
            System.Console.WriteLine(source);
        }

        System.Console.WriteLine();
    }
}
