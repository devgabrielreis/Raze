namespace Raze.Script.Core.Metadata;

public class SourceInfo
{
    public SourcePosition? SourcePosition { get; private set; }

    public string Location { get; private set; }

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
