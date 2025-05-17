namespace Raze.Script.Core.Exceptions;

public class LexerException : Exception
{
    public int Line;
    public int Column;

    public LexerException(string message, int line, int column) : base(message)
    {
        Line = line;
        Column = column;
    }

    public override string ToString()
    {
        return $"{this.GetType().Name} -> {Message}. At line {Line}, column {Column}.";
    }
}
