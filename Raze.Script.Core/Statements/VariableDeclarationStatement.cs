using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Types;

namespace Raze.Script.Core.Statements;

internal class VariableDeclarationStatement : Statement
{
    public string Identifier { get; private set; }
    public Expression? Value { get; private set; }
    public RuntimeType Type { get; private set; }
    public bool IsConstant { get; private set; }

    public VariableDeclarationStatement(string identifier, RuntimeType type, Expression? value, bool isConstant, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        Identifier = identifier;
        Value = value;
        Type = type;
        IsConstant = isConstant;
    }
}
