using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal sealed class NamespaceAccessExpression : Expression
{
    internal readonly IdentifierExpression NamespaceIdentifier;
    internal readonly IdentifierExpression MemberIdentifier;

    internal NamespaceAccessExpression(
        IdentifierExpression namespaceIdentifier,
        IdentifierExpression memberIdentifier,
        ref readonly SourceInfo source
    )
        : base(in source, true)
    {
        NamespaceIdentifier = namespaceIdentifier;
        MemberIdentifier = memberIdentifier;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitNamespaceAccessExpression(this, state, out result);
    }
}
