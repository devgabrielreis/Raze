namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class RedeclarationException : RuntimeException
{
    public RedeclarationException(string error, int? line, int? column)
        : base(error, line, column, nameof(RedeclarationException))
    {
    }
}
