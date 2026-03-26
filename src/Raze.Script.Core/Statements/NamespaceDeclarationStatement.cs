using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal sealed class NamespaceDeclarationStatement : Statement
{
    internal string Identifier { get; private set; }
    internal CodeBlockStatement DeclarationBlock { get; private set; }

    internal NamespaceDeclarationStatement(string identifier, CodeBlockStatement declarationBlock, SourceInfo source)
        : base(source, false)
    {
        Identifier = identifier;
        DeclarationBlock = declarationBlock;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitNamespaceDeclarationStatement(this, state);
    }
}
