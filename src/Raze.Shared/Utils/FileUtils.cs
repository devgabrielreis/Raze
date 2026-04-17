namespace Raze.Shared.Utils;

public static class FileUtils
{
    public static bool TryReadAllFileLines(FileInfo file, out string fileContent, out string errorMessage)
    {
        try
        {
            fileContent = File.ReadAllText(file.FullName);
            errorMessage = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            fileContent = string.Empty;
            errorMessage = ex.Message;
            return false;
        }
    }

    public static bool TryReadFileLine(FileInfo file, int lineIndex, out string lineContent, out string errorMessage)
    {
        try
        {
            var content = File.ReadLines(file.FullName).Skip(lineIndex).FirstOrDefault();

            if (content == null)
            {
                lineContent = string.Empty;
                errorMessage = $"Line {lineIndex} not found on file {file.FullName}";
                return false;
            }

            lineContent = content;
            errorMessage = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            lineContent = string.Empty;
            errorMessage = ex.Message;
            return false;
        }
    }
}
