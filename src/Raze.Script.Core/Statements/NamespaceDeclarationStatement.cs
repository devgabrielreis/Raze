using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements;

internal sealed class NamespaceDeclarationStatement : Statement
{
    internal readonly string Identifier;
    internal readonly CodeBlockStatement DeclarationBlock;

    internal NamespaceDeclarationStatement(
        string identifier,
        CodeBlockStatement declarationBlock,
        ref readonly SourceInfo source
    )
        : base(in source, false)
    {
        Identifier = identifier;
        DeclarationBlock = declarationBlock;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitNamespaceDeclarationStatement(this, state, out result);
    }
}
