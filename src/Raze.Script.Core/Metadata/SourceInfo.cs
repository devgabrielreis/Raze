namespace Raze.Script.Core.Metadata;

public readonly struct SourceInfo
{
    public readonly SourcePosition? SourcePosition;
    public readonly string Location;

    public SourceInfo(string location)
    {
        SourcePosition = null;
        Location = location;
    }

    internal SourceInfo(SourcePosition position, string location)
    {
        SourcePosition = position;
        Location = location;
    }
}
