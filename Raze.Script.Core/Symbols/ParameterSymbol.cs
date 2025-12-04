using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Types;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Symbols;

public class ParameterSymbol : Symbol
{
    public bool IsConstant { get; private set; }

    public RuntimeType Type { get; private set; }

    public string Identifier { get; private set; }

    internal Expression? DefaultValue { get; private set; }

    public RuntimeValue? DefaultRuntimeValue { get; private set; }

    public int? StartLine { get; private set; }

    public int? StartColumn { get; private set; }

    public ParameterSymbol(bool isConstant, RuntimeType type, string identifier, RuntimeValue? defaultValue)
    {
        IsConstant = isConstant;
        Type = type;
        Identifier = identifier;
        DefaultRuntimeValue = defaultValue;
        StartLine = null;
        StartColumn = null;
    }

    internal ParameterSymbol(
        bool isConstant,
        RuntimeType type,
        string identifier,
        Expression? defaultValue,
        int startLine,
        int startColumn
    )
    {
        IsConstant = isConstant;
        Type = type;
        Identifier = identifier;
        DefaultValue = defaultValue;
        StartLine = startLine;
        StartColumn = startColumn;
    }
}
