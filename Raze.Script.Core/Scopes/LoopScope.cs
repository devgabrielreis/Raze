namespace Raze.Script.Core.Scopes;

internal class LoopScope : Scope
{
    protected override bool CanDeclareConstants => false;
    protected override bool CanDeclareVariables => true;

    public LoopScope(Scope parent)
        : base(parent)
    {
    }
}
