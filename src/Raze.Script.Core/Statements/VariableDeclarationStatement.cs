using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal sealed class VariableDeclarationStatement : Statement
{
    internal string Identifier { get; private set; }
    internal Expression? Value { get; private set; }
    internal RuntimeType Type { get; private set; }
    internal bool IsConstant { get; private set; }

    internal VariableDeclarationStatement(string identifier, RuntimeType type, Expression? value, bool isConstant, SourceInfo source)
        : base(source, true)
    {
        Identifier = identifier;
        Value = value;
        Type = type;
        IsConstant = isConstant;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitVariableDeclarationStatement(this, state);
    }
}
