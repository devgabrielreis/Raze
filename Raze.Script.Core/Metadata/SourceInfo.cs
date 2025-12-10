namespace Raze.Script.Core.Metadata;

public class SourceInfo
{
    public int? StartLine { get; private set; }

    public int? StartColumn { get; private set; }

    public string Location { get; private set; }

    public SourceInfo(string location)
        : this(null, null, location)
    {
    }

    internal SourceInfo(int? startLine, int? startColumn, string location)
    {
        StartLine = startLine;
        StartColumn = startColumn;
        Location = location;
    }
}
