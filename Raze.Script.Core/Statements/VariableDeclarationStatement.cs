using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Types;

namespace Raze.Script.Core.Statements;

internal class VariableDeclarationStatement : Statement
{
    public string Identifier { get; private set; }
    public Expression? Value { get; private set; }
    public RuntimeType Type { get; private set; }
    public bool IsConstant { get; private set; }

    public VariableDeclarationStatement(string identifier, RuntimeType type, Expression? value, bool isConstant, SourceInfo source)
        : base(source)
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
