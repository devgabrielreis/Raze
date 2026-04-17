using Raze.Script.Core.Exceptions;
using Raze.Shared.Utils;

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
        var sourcePosition = ex.SourceInfo.SourcePosition;
        string? errorLine = null;

        if (
            sourcePosition != null
            && sources.TryGetValue(ex.SourceInfo.Location, out string? source)
        )
        {
            errorLine = source.Split('\n')[sourcePosition.Value.Line - 1];
        }

        PrettyPrintRazeException(ex, errorLine);
    }

    internal static void PrettyPrintRazeException(RazeException ex)
    {
        var sourcePosition = ex.SourceInfo.SourcePosition;
        string? errorLine = null;

        if (
            sourcePosition != null
            && FileUtils.TryReadFileLine(
                new FileInfo(ex.SourceInfo.Location),
                sourcePosition.Value.Line - 1,
                out var fileErrorLine,
                out _
            )
        )
        {
            errorLine = fileErrorLine;
        }

        PrettyPrintRazeException(ex, errorLine);
    }

    private static void PrettyPrintRazeException(RazeException ex, string? errorLine)
    {
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine(ex.Message);
        System.Console.ResetColor();
        System.Console.WriteLine();

        System.Console.Write("At ");
        System.Console.ForegroundColor = ConsoleColor.Magenta;
        System.Console.WriteLine(ex.SourceInfo.Location);
        System.Console.ResetColor();

        var sourcePosition = ex.SourceInfo.SourcePosition;

        if (sourcePosition != null)
        {
            System.Console.WriteLine($"Line {sourcePosition.Value.Line}, column {sourcePosition.Value.Column}.");
            System.Console.WriteLine();
        }

        if (errorLine != null)
        {
            System.Console.WriteLine(errorLine);
        }

        if (sourcePosition != null && errorLine != null && errorLine.Length >= sourcePosition.Value.Column)
        {
            for (int i = 0; i < sourcePosition.Value.Column - 1; i++)
            {
                System.Console.Write(errorLine[i] == '\t' ? '\t' : ' ');
            }

            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine('^');
            System.Console.ResetColor();
        }

        System.Console.WriteLine();
    }
}
