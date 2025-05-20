namespace Raze.Script.Core.Scopes;

public class InterpreterScope : Scope
{
    protected override bool CanDeclareConstants => true;
    protected override bool CanDeclareVariables => true;

    public InterpreterScope()
        : base(null)
    {
    }
}
