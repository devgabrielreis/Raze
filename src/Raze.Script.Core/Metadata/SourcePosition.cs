namespace Raze.Script.Core.Metadata;

public readonly struct SourcePosition
{
    public readonly int Line;
    public readonly int Column;

    internal SourcePosition(int line, int column)
    {
        Line = line;
        Column = column;
    }
}
