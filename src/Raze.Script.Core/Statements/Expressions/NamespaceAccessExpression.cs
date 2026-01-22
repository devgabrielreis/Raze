using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal class NamespaceAccessExpression : Expression
{
    public IdentifierExpression NamespaceIdentifier { get; private set; }
    public IdentifierExpression MemberIdentifier { get; private set; }

    public NamespaceAccessExpression(
        IdentifierExpression namespaceIdentifier, IdentifierExpression memberIdentifier ,SourceInfo source
    )
        : base(source)
    {
        NamespaceIdentifier = namespaceIdentifier;
        MemberIdentifier = memberIdentifier;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitNamespaceAccessExpression(this, state);
    }
}
