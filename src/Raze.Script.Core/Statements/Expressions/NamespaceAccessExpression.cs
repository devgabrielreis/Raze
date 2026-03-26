using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal sealed class NamespaceAccessExpression : Expression
{
    internal IdentifierExpression NamespaceIdentifier { get; private set; }
    internal IdentifierExpression MemberIdentifier { get; private set; }

    internal NamespaceAccessExpression(
        IdentifierExpression namespaceIdentifier, IdentifierExpression memberIdentifier ,SourceInfo source
    )
        : base(source, true)
    {
        NamespaceIdentifier = namespaceIdentifier;
        MemberIdentifier = memberIdentifier;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitNamespaceAccessExpression(this, state);
    }
}
