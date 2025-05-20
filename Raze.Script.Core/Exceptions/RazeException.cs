namespace Raze.Script.Core.Exceptions;

public abstract class RazeException : Exception
{
    public int? Line { get; private set; }
    public int? Column { get; private set; }

    public RazeException(string error, int? line, int? column, string className)
        : base($"{className} -> {error}." + ((line is not null && column is not null) ? $" At line {line}, column {column}." : ""))
    {
        Line = line;
        Column = column;
    }

    public RazeException(int? line, int? column, string className)
        : base($"{className}." + ((line is not null && column is not null) ? $" At line {line}, column {column}." : ""))
    {
        Line = line;
        Column = column;
    }
}
