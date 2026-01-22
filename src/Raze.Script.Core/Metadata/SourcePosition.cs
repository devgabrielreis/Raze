namespace Raze.Script.Core.Metadata;

public class SourcePosition
{
    public int Line { get; private set; }

    public int Column { get; private set; }

    internal SourcePosition(int line, int column)
    {
        Line = line;
        Column = column;
    }
}
