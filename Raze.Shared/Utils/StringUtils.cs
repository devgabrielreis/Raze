namespace Raze.Shared.Utils;

public static class StringUtils
{
    public static string UnescapeString(string str)
    {
        return str
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n")
                .Replace("\t", "\\t")
                .Replace("\r", "\\r");
    }
}
