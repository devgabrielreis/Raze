namespace Raze.Script.Core.Metadata;

public readonly struct SourceInfo
{
    public readonly SourcePosition? SourcePosition;
    public readonly string Location;

    public SourceInfo(string location)
        : this(null, location)
    {
    }

    internal SourceInfo(SourcePosition? position, string location)
    {
        SourcePosition = position;
        Location = location;
    }
}
