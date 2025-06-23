using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens.Primitives;

namespace Raze.Script.Core.Statements;

internal class VariableDeclarationStatement : Statement
{
    public string Identifier { get; private set; }
    public Expression? Value { get; private set; }
    public PrimitiveTypeToken Type { get; private set; }
    public bool IsConstant { get; private set; }
    public bool IsNullable { get; private set; }

    public VariableDeclarationStatement(string identifier, PrimitiveTypeToken type, Expression? value, bool isConstant, bool isNullable, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        Identifier = identifier;
        Value = value;
        Type = type;
        IsConstant = isConstant;
        IsNullable = isNullable;
    }
}
