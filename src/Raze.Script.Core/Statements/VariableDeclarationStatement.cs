using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal sealed class VariableDeclarationStatement : Statement
{
    internal readonly string Identifier;
    internal readonly Expression? Value;
    internal readonly RuntimeType Type;
    internal readonly bool IsConstant;

    internal VariableDeclarationStatement(
        string identifier,
        RuntimeType type,
        Expression? value,
        bool isConstant,
        ref readonly SourceInfo source
    )
        : base(in source, true)
    {
        Identifier = identifier;
        Value = value;
        Type = type;
        IsConstant = isConstant;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitVariableDeclarationStatement(this, state, out result);
    }
}
