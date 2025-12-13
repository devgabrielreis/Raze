using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Metadata;
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

    public SourceInfo SourceInfo { get; private set; }

    public ParameterSymbol(
        bool isConstant, RuntimeType type, string identifier, SourceInfo source, RuntimeValue? defaultValue
    )
        : this(
            isConstant,
            type,
            identifier,
            (defaultValue is null ? null : new RuntimeValueExpression(defaultValue, source)),
            source
        )
    {
    }

    internal ParameterSymbol(
        bool isConstant, RuntimeType type, string identifier, Expression? defaultValue, SourceInfo source
    )
    {
        if (type is VoidType)
        {
            throw new InvalidSymbolDeclarationException("Parameter cannot have void type", source);
        }

        IsConstant = isConstant;
        Type = type;
        Identifier = identifier;
        DefaultValue = defaultValue;
        SourceInfo = source;
    }

    public bool Equivalent(ParameterSymbol other)
    {
        return other.Type.Equals(Type)
            && other.HasDefaultValue() == HasDefaultValue();
    }

    public bool HasDefaultValue()
    {
        return DefaultValue is not null;
    }
}
