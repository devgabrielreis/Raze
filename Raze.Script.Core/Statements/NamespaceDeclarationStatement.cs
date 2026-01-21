using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal class NamespaceDeclarationStatement : Statement
{
    public string Identifier { get; private set; }
    public CodeBlockStatement DeclarationBlock { get; private set; }

    public override bool RequireSemicolon => false;

    public NamespaceDeclarationStatement(string identifier, CodeBlockStatement declarationBlock, SourceInfo source)
        : base(source)
    {
        Identifier = identifier;
        DeclarationBlock = declarationBlock;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitNamespaceDeclarationStatement(this, state);
    }
}
