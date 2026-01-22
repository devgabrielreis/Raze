namespace Raze.Script.Core.Runtime.Scopes;

internal class NamespaceScope : Scope
{
    protected override bool CanDeclareConstants => true;
    protected override bool CanDeclareVariables => false;
    protected override bool CanDeclareNamespaces => false;

    public NamespaceScope(Scope parent)
        : base(parent)
    {
    }
}
