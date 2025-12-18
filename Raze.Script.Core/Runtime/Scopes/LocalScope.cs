namespace Raze.Script.Core.Runtime.Scopes;

internal class LocalScope : Scope
{
    protected override bool CanDeclareConstants => true;
    protected override bool CanDeclareVariables => true;

    public LocalScope(Scope parent)
        : base(parent)
    {
    }
}
