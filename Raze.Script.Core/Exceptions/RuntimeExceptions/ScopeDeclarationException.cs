namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class ScopeDeclarationException : RuntimeException
{
    public ScopeDeclarationException(string structureName, string scopeType, int? line, int? column)
        : base($"Cannot declare {structureName} on {scopeType}", line, column, nameof(ScopeDeclarationException))
    {
    }
}
